using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Requests;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class HydrologicalForeCastRepository : BaseRepository<HydrologicalForeCast>, IHydrologicalForeCastRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public HydrologicalForeCastRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> DownloadDataAsync(List<HydrologicalForeCast> hydrologicalForeCasts, DownloadDataRequest request)
        {
            var fromDate = hydrologicalForeCasts.Min(x => x.RefDate);
            var toDate = hydrologicalForeCasts.Max(x => x.RefDate);
            var existingHydrologicalForeCasts =
                await _unitOfWork.HydrologicalForeCastRepository.GetWhereAsync(x => x.RefDate >= fromDate
                                                                                    && x.RefDate <= toDate);

            foreach (var itemDownloaded in hydrologicalForeCasts)
            {
                var entry =
                    existingHydrologicalForeCasts.FirstOrDefault(x => x.StationId.Equals(itemDownloaded.StationId)
                                                                      && x.RefDate.Equals(itemDownloaded.RefDate));

                var minValue = itemDownloaded.MinValue == -9990 || itemDownloaded.MinValue == -9999
                    ? null
                    : itemDownloaded.MinValue; 
                var maxValue = itemDownloaded.MaxValue == -9990 || itemDownloaded.MaxValue == -9999
                    ? null
                    : itemDownloaded.MaxValue;
                var isUpdate = false;

                if (entry != null)
                {
                    if (entry.MaxValue != maxValue)
                    {
                        entry.MaxValue = maxValue;
                        isUpdate = true;
                    }

                    if (entry.MinValue != minValue)
                    {
                        entry.MinValue = minValue;
                        isUpdate = true;
                    }

                    if (entry.Type != itemDownloaded.Type)
                    {
                        entry.Type = itemDownloaded.Type;
                        isUpdate = true;
                    }

                    if (isUpdate)
                    {
                        _unitOfWork.HydrologicalForeCastRepository.Update(entry);
                    }
                }
                else
                {
                    entry = new HydrologicalForeCast()
                    {
                        RefDate = itemDownloaded.RefDate,
                        Id = Guid.NewGuid(),
                        StationId = itemDownloaded.StationId,
                        MaxValue = maxValue,
                        Type = itemDownloaded.Type,
                        MinValue = minValue
                    };
                    _unitOfWork.HydrologicalForeCastRepository.Add(entry);
                }
            }

            return await _unitOfWork.CommitAsync();
        }
    }
}