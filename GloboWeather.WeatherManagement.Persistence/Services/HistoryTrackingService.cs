using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Domain.Entities.Social;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace GloboWeather.WeatherManagement.Persistence.Services
{
    public class HistoryTrackingService : IHistoryTrackingService
    {
        private readonly IServiceProvider _serviceProvider;

        public HistoryTrackingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SaveAsync(HistoryTracking entry)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var postService = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                postService.HistoryTrackingRepository.Add(entry);
                await postService.CommitAsync();
            }
            catch
            {
                //Log
            }
        }

        public async Task SaveAsync(string objectName, object originalData, object updatedData, string description, string ipAddress)
        {
            try
            {
                var originalString = originalData != null ? JsonConvert.SerializeObject(originalData) : string.Empty;
                var updatedString = updatedData != null ? JsonConvert.SerializeObject(updatedData) : string.Empty;
                var entry = new HistoryTracking
                {
                    ObjectName = objectName,
                    OriginalData = originalString,
                    Id = Guid.NewGuid(),
                    Description = description,
                    UpdatedData = updatedString,
                    IpAddress = ipAddress
                };

                using var scope = _serviceProvider.CreateScope();
                var postService = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                postService.HistoryTrackingRepository.Add(entry);
                await postService.CommitAsync();
            }
            catch
            {
                //Log
            }
        }
    }
}
