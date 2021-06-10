using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.CreateWeatherInformation;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
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
            var createEventCommand = new CreateWeatherInformationCommand()
            {
                StationId = "Test",

            };

            _timer = new Timer(async state =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IWeatherInformationRepository>();
                    await service.AddAsync(new Domain.Entities.WeatherInformation());

                }


            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));

        }
    }
}
