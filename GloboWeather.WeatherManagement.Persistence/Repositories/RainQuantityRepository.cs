using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Features.RainQuantities.Import;
using GloboWeather.WeatherManagement.Application.Requests;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class RainQuantityRepository : BaseRepository<RainQuantity>, IRainQuantityRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public RainQuantityRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> DownloadDataAsync(List<RainQuantity> rainQuantities, DownloadDataRequest request)
        {
            var fromDate = rainQuantities.Min(x => x.RefDate);
            var toDate = rainQuantities.Max(x => x.RefDate);
            var existingRainQuantitys =
                await _unitOfWork.RainQuantityRepository.GetWhereAsync(x => x.RefDate >= fromDate
                                                                                    && x.RefDate <= toDate);

            foreach (var itemDownloaded in rainQuantities)
            {
                var entry =
                    existingRainQuantitys.FirstOrDefault(x => x.StationId.Equals(itemDownloaded.StationId)
                                                                      && x.RefDate.Equals(itemDownloaded.RefDate));

                var value = itemDownloaded.Value == -9990 || itemDownloaded.Value == -9999
                    ? null
                    : itemDownloaded.Value;
                if (entry != null)
                {
                    if (entry.Value != value)
                    {
                        entry.Value = value;
                        _unitOfWork.RainQuantityRepository.Update(entry);
                    }
                }
                else
                {
                    entry = new RainQuantity()
                    {
                        Id = Guid.NewGuid(),
                        StationId = itemDownloaded.StationId,
                        RefDate = itemDownloaded.RefDate,
                        Value = value
                    };
                    _unitOfWork.RainQuantityRepository.Add(entry);
                }
            }

            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> ImportAsync(ImportRainQuantityCommand request, List<RainQuantity> rainQuantities, CancellationToken cancellationToken)
        {
            var fromDate = rainQuantities.Min(x => x.RefDate);
            var toDate = rainQuantities.Max(x => x.RefDate);
            var listStation = rainQuantities.Select(x => x.StationId).Distinct();

            var existingItems = await _unitOfWork.RainQuantityRepository
                .GetWhereAsync(x => listStation.Contains(x.StationId)
                                    && x.RefDate >= fromDate && x.RefDate <= toDate, cancellationToken);

            foreach (var entry in rainQuantities)
            {
                var rainQuantity =
                    existingItems.FirstOrDefault(x => x.StationId == entry.StationId && x.RefDate == entry.RefDate);
                if (rainQuantity == null)
                {
                    rainQuantity = new RainQuantity()
                    {
                        RefDate = entry.RefDate,
                        Id = Guid.NewGuid(),
                        StationId = entry.StationId,
                        Value = entry.Value
                    };
                    _unitOfWork.RainQuantityRepository.Add(rainQuantity);
                }
                else
                {
                    var isUpdate = false;
                    if (!rainQuantity.Value.Equals(entry.Value))
                    {
                        rainQuantity.Value = entry.Value;
                        isUpdate = true;
                    }

                    if (isUpdate)
                    {
                        _unitOfWork.RainQuantityRepository.Update(rainQuantity);
                    }
                }
            }

            return await _unitOfWork.CommitAsync();
        }
    }
}