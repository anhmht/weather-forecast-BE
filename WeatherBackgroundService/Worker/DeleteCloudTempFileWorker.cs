using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherBackgroundService.Worker
{
    public class DeleteCloudTempFileWorker : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IImageService _imageService;
        private readonly IServiceProvider _serviceProvider;
        public DeleteCloudTempFileWorker(IServiceProvider serviceProvider, IConfiguration configuration, IImageService imageService)
        {
            _configuration = configuration;
            _imageService = imageService;
            _serviceProvider = serviceProvider;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
#if DEBUG
            //Skip this service when debug
            //return;
#endif
            var interval = _configuration.GetSection("BackgroundWorkerConfigs:DeleteCloudTempFileEveryHours").Get<int>();
            new Timer(async state =>
            {
                //Work at 00h00 AM after application start
                var current = DateTime.Now;
                var startTime = DateTime.Now.Date.AddDays(1);
                var waitTime = startTime - current;

                await Task.Delay((int)waitTime.TotalMilliseconds, cancellationToken);

                //Delete in temp folder
                try
                {
                    await _imageService.DeleteAllImagesTempContainerAsync();
                }
                catch (Exception e)
                {
                    //Log
                }

                //Delete temp file of the Post on social
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var postService = scope.ServiceProvider.GetRequiredService<IPostService>();
                    await postService.DeleteTempFile();
                }
                catch (Exception e)
                {
                    //Log
                }

            }, null, TimeSpan.Zero, TimeSpan.FromHours(interval));
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
