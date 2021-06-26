using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherBackgroundService.Worker
{
    public class AutoGenerateCacheWorker : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        public AutoGenerateCacheWorker(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var interval = _configuration.GetSection("BackgroundWorkerConfigs:AutoGenerateCacheCyclic").Get<int>();
            var listGetWeatherInformationRequest = _configuration
                .GetSection("BackgroundWorkerConfigs:AutoGenerateCacheRequests:GetWeatherInformationRequest")
                .Get<string[]>();
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var listGetWeatherInformationHorizontalRequest = _configuration
                .GetSection(
                    "BackgroundWorkerConfigs:AutoGenerateCacheRequests:GetWeatherInformationHorizontalRequest")
                .Get<string[]>();
            new Timer(async state =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var weatherInformationService = scope.ServiceProvider.GetRequiredService<IWeatherInformationService>();
                    foreach (var weatherInformationRequest in listGetWeatherInformationRequest)
                    {
                        try
                        {
                            var request =
                                JsonSerializer.Deserialize<GetWeatherInformationRequest>(weatherInformationRequest, jsonSerializerOptions);
                            request.FromDate = DateTime.Now.GetStartOfDate();
                            request.ToDate = DateTime.Now.GetEndOfDate();
                            await weatherInformationService.GetWeatherInformationsAsync(request, cancellationToken);
                        }
                        catch (Exception e)
                        {
                            //Write log
                            //throw;
                        }
                    }

                    foreach (var weatherInformationRequest in listGetWeatherInformationHorizontalRequest)
                    {
                        try
                        {
                            var request =
                            JsonSerializer.Deserialize<GetWeatherInformationHorizontalRequest>(weatherInformationRequest, jsonSerializerOptions);
                            request.FromDate = DateTime.Now.GetStartOfDate();
                            request.ToDate = DateTime.Now.GetEndOfDate();
                            await weatherInformationService.GetWeatherInformationHorizontalAsync(request,
                                cancellationToken);
                        }
                        catch (Exception e)
                        {
                            //Write log
                            //throw;
                        }
                    }
                }
            }, null, TimeSpan.Zero, TimeSpan.FromHours(interval));
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
