﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManegement.Application.Contracts.Media;

namespace WeatherBackgroundService.Worker
{
    public class DeleteCloudTempFileWorker : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IImageService _imageService;
        public DeleteCloudTempFileWorker(IServiceProvider serviceProvider, IConfiguration configuration, IImageService imageService)
        {
            _configuration = configuration;
            _imageService = imageService;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
#if DEBUG
            //Skip this service when debug
            return;
#endif
            var interval = _configuration.GetSection("BackgroundWorkerConfigs:DeleteCloudTempFileEveryHours").Get<int>();
            new Timer(async state =>
            {
                await Task.Delay(20000, cancellationToken); //Work after application start 20 seconds
                try
                {
                    await _imageService.DeleteAllImagesTempContainerAsync();
                }
                catch (Exception e)
                {
                    throw;
                }
            }, null, TimeSpan.Zero, TimeSpan.FromHours(interval));
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
