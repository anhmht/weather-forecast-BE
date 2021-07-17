using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherBackgroundService.Worker
{
    public class DownloadMonitoringDataWorker : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        public DownloadMonitoringDataWorker(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
#if DEBUG
            //Skip this service when debug
            return;
#endif
            var interval = _configuration.GetSection("BackgroundWorkerConfigs:DownLoadMonitoringData").Get<int>();
            new Timer(async state =>
            {
                await Task.Delay(40000, cancellationToken); //Work after application start 40 seconds
                using (var scope = _serviceProvider.CreateScope())
                {
                    var downloadDataService = scope.ServiceProvider.GetRequiredService<IDownloadDataService>();
                    var downloadDataRequest = new DownloadDataRequest()
                    {
                        //FromDate = new DateTime(2021, 07, 03),
                        //ToDate = new DateTime(2021, 07, 03)
                        FromDate = DateTime.Now.Date,
                        ToDate = DateTime.Now.Date
                    };
                    await downloadDataService.DownloadDataAsync(downloadDataRequest);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromHours(interval));
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
