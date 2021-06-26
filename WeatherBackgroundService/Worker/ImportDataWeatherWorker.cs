using System;
using System.Collections.Generic;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace WeatherBackgroundService.Worker
{
    public class ImportDataWeatherWorker : IHostedService
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        public ImportDataWeatherWorker(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }


        public  async Task StartAsync(CancellationToken cancellationToken)
        {
            var timeToRun = _configuration.GetSection("SyncWeatherDataSettings:RunTime").Get<DateTime[]>().ToList();
            _timer = new Timer(async state =>
            {
                if (timeToRun.Select(x => x.ToString("HH:mm")).Contains(DateTime.Now.ToString("HH:mm")))
                {

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var _windLevelService = scope.ServiceProvider.GetRequiredService<IWindLevelService>();
                        var _humidityService = scope.ServiceProvider.GetRequiredService<IHumidityService>();
                        var _rainAmountService = scope.ServiceProvider.GetRequiredService<IRainAmountService>();
                        var _temperatureService = scope.ServiceProvider.GetRequiredService<ITemperatureService>();
                        var _weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
                        var _windSpeedService = scope.ServiceProvider.GetRequiredService<IWindSpeedService>();
                        var _windDirectionService = scope.ServiceProvider.GetRequiredService<IWindDirectionService>();
                        var _backgroundServiceTrackingRepository = scope.ServiceProvider.GetRequiredService<IBackgroundServiceTrackingRepository>();
                        var _weatherInformationRepository = scope.ServiceProvider.GetRequiredService<IWeatherInformationRepository>();
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var windRankService = scope.ServiceProvider.GetRequiredService<IWindRankService>();
                        var lastImportTimes = await _backgroundServiceTrackingRepository.GetLastBackgroundServiceTracking();
                        var importTime = DateTime.MinValue;
                        if (lastImportTimes != null)
                            importTime = lastImportTimes.LastDownload;
                        try
                        {
                            // sync winlevel
                            var winLevels = await _windLevelService.ListAllAsync();
                            await _weatherInformationRepository.SyncWinLevelAsync(winLevels, importTime, true);

                            // sync Humidity
                            var humidities = await _humidityService.ListAllAsync();
                            await _weatherInformationRepository.SyncHumidityAsync(humidities, importTime, true);

                            // sync winlevel
                            var rainAmount = await _rainAmountService.ListAllAsync();
                            await _weatherInformationRepository.SyncRainAmountAsync(rainAmount, importTime, true);

                            // sync temperature
                            var temperature = await _temperatureService.ListAllAsync();
                            await _weatherInformationRepository.SyncTemperatureAsync(temperature, importTime, true);

                            // sync weather
                            var weather = await _weatherService.ListAllAsync();
                            await _weatherInformationRepository.SyncWeatherAsync(weather, importTime, true);

                            // sync windSpeed
                            var windSpeeds = await _windSpeedService.ListAllAsync();
                            await _weatherInformationRepository.SyncWindSpeedAsync(windSpeeds, importTime, true);

                            // sync windSpeed
                            var windDirections = await _windDirectionService.ListAllAsync();
                            await _weatherInformationRepository.SyncWindDirectionAsync(windDirections, importTime, true);

                            var listImportTime = new List<DateTime>();
                            listImportTime.Add(winLevels.Min(x => x.RefDate));
                            listImportTime.Add(humidities.Min(x => x.RefDate));
                            listImportTime.Add(rainAmount.Min(x => x.RefDate));
                            listImportTime.Add(temperature.Min(x => x.RefDate));
                            listImportTime.Add(weather.Min(x => x.RefDate));
                            listImportTime.Add(windSpeeds.Min(x => x.RefDate));
                            listImportTime.Add(windDirections.Min(x => x.RefDate));

                            var minImportTime = listImportTime.Min();
                            unitOfWork.BackgroundServiceTrackingRepository.Add(new BackgroundServiceTracking()
                            {
                                LastDownload = minImportTime,
                                LastUpdate = DateTime.Now
                            });
                            await unitOfWork.CommitAsync();

                            //Download windRank
                            await windRankService.DownloadAsync();
                        }
                        catch (Exception ex)
                        {
                            //Log
                            //throw;
                        }
                    }
                }

            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
