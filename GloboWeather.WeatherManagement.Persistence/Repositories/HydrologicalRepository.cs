using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Features.Hydrologicals.Import;
using GloboWeather.WeatherManagement.Application.Requests;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class HydrologicalRepository : BaseRepository<Hydrological>, IHydrologicalRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public HydrologicalRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> DownloadDataAsync(List<Hydrological> hydrologicals, DownloadDataRequest request)
        {
            var fromDate = hydrologicals.Min(x => x.Date);
            var toDate = hydrologicals.Max(x => x.Date);
            var existingHydrologicals =
                await _unitOfWork.HydrologicalRepository.GetWhereAsync(x => x.Date >= fromDate
                                                                                    && x.Date <= toDate);

            foreach (var itemDownloaded in hydrologicals)
            {
                var entry =
                    existingHydrologicals.FirstOrDefault(x => x.StationId.Equals(itemDownloaded.StationId)
                                                                      && x.Date.Equals(itemDownloaded.Date));
                var accumulated = itemDownloaded.Accumulated == -9990 || itemDownloaded.Accumulated == -9999
                    ? null
                    : itemDownloaded.Accumulated;
                var rain = itemDownloaded.Rain == -9990 || itemDownloaded.Rain == -9999
                    ? null
                    : itemDownloaded.Rain;
                var waterLevel = itemDownloaded.WaterLevel == -9990 || itemDownloaded.WaterLevel == -9999
                    ? null
                    : itemDownloaded.WaterLevel;
                var isUpdate = false;

                if (entry != null)
                {
                    if (entry.Accumulated != accumulated)
                    {
                        entry.Accumulated = accumulated;
                        isUpdate = true;
                    }

                    if (entry.Rain != rain)
                    {
                        entry.Rain = rain;
                        isUpdate = true;
                    }

                    if (entry.WaterLevel != waterLevel)
                    {
                        entry.WaterLevel = waterLevel;
                        isUpdate = true;
                    }


                    if (isUpdate)
                    {
                        _unitOfWork.HydrologicalRepository.Update(entry);
                    }
                }
                else
                {
                    entry = new Hydrological()
                    {
                        Id = Guid.NewGuid(),
                        StationId = itemDownloaded.StationId,
                        Date = itemDownloaded.Date,
                        Accumulated = accumulated,
                        Rain = rain,
                        WaterLevel = waterLevel
                    };
                    _unitOfWork.HydrologicalRepository.Add(entry);
                }
            }

            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> ImportAsync(ImportHydrologicalCommand request, List<Hydrological> hydrologicals, CancellationToken cancellationToken)
        {
            var fromDate = hydrologicals.Min(x => x.Date);
            var toDate = hydrologicals.Max(x => x.Date);
            var listStation = hydrologicals.Select(x => x.StationId).Distinct();

            var existingItems = await _unitOfWork.HydrologicalRepository
                .GetWhereAsync(x => listStation.Contains(x.StationId)
                                    && x.Date >= fromDate && x.Date <= toDate, cancellationToken);


            foreach (var entry in hydrologicals)
            {
                var hydrological =
                    existingItems.FirstOrDefault(x => x.StationId == entry.StationId && x.Date == entry.Date);
                if (hydrological == null)
                {
                    hydrological = new Hydrological()
                    {
                        Date = entry.Date,
                        Id = Guid.NewGuid(),
                        StationId = entry.StationId,
                        Rain = entry.Rain,
                        WaterLevel = entry.WaterLevel,
                        Accumulated = entry.Accumulated
                    };
                    _unitOfWork.HydrologicalRepository.Add(hydrological);
                }
                else
                {
                    var isUpdate = false;
                    if (!hydrological.Rain.Equals(entry.Rain))
                    {
                        hydrological.Rain = entry.Rain;
                        isUpdate = true;
                    }
                    if (!hydrological.WaterLevel.Equals(entry.WaterLevel))
                    {
                        hydrological.WaterLevel = entry.WaterLevel;
                        isUpdate = true;
                    }
                    if (!hydrological.Accumulated.Equals(entry.Accumulated))
                    {
                        hydrological.Accumulated = entry.Accumulated;
                        isUpdate = true;
                    }

                    if (isUpdate)
                    {
                        _unitOfWork.HydrologicalRepository.Update(hydrological);
                    }
                }
            }

            return await _unitOfWork.CommitAsync();
        }
    }
}