using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.CreateWeatherInformation;
using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Api.Worker
{
    public class ImportDataWeatherWorker : BackgroundService
    {
        private Timer _timer;
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        public ImportDataWeatherWorker(IMediator mediator, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(async state =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _windLevelService = scope.ServiceProvider.GetRequiredService<IWindLevelService>();
                    var _backgroundServiceTrackingRepository = scope.ServiceProvider.GetRequiredService<IBackgroundServiceTrackingRepository>();
                    var _weatherInformationRepository = scope.ServiceProvider.GetRequiredService<IWeatherInformationRepository>();
                    var importTimes = await _backgroundServiceTrackingRepository.ListAllAsync();
                    var importTime = DateTime.MinValue;
                    if (importTimes.Any())
                        importTime = importTimes.FirstOrDefault().LastDownload;
                    try
                    {
                        var winLevels = await _windLevelService.ListAllAsync();
                        await _weatherInformationRepository.UpdateWinLevelAsync(winLevels, importTime);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                 
                }               

            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10000));

        }
    }
}
