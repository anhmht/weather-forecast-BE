using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Requests;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManagement.Monitoring.IRepository;

namespace GloboWeather.WeatherManagement.Monitoring.Services
{
    public class DownloadDataService : IDownloadDataService
    {
        private readonly IMapper _mapper;
        private readonly IHydrologicalForecastRepository _hydrologicalForecastRepository;
        private readonly IHydrologicalRepository _hydrologicalRepository;
        private readonly IRainRepository _rainRepository;
        private readonly IMeteorologicalRepository _meteorologicalRepository;

        private readonly Application.Contracts.Persistence.IHydrologicalForeCastRepository
            _pHydrologicalForecastRepository;
        private readonly Application.Contracts.Persistence.IHydrologicalRepository
            _pHydrologicalRepository;
        private readonly Application.Contracts.Persistence.IRainQuantityRepository
            _pRainQuantityRepository;
        private readonly Application.Contracts.Persistence.IMeteorologicalRepository
            _pMeteorologicalRepository;

        public DownloadDataService(IMapper mapper,
            IHydrologicalForecastRepository hydrologicalForecastRepository,
            Application.Contracts.Persistence.IHydrologicalForeCastRepository pHydrologicalForecastRepository,
            IHydrologicalRepository hydrologicalRepository,
            Application.Contracts.Persistence.IHydrologicalRepository pHydrologicalRepository,
            IRainRepository rainRepository,
            Application.Contracts.Persistence.IRainQuantityRepository pRainQuantityRepository,
            IMeteorologicalRepository meteorologicalRepository,
            Application.Contracts.Persistence.IMeteorologicalRepository pMeteorologicalRepository
        )
        {
            _mapper = mapper;
            _hydrologicalForecastRepository = hydrologicalForecastRepository;
            _pHydrologicalForecastRepository = pHydrologicalForecastRepository;
            _hydrologicalRepository = hydrologicalRepository;
            _pHydrologicalRepository = pHydrologicalRepository;
            _rainRepository = rainRepository;
            _pRainQuantityRepository = pRainQuantityRepository;
            _meteorologicalRepository = meteorologicalRepository;
            _pMeteorologicalRepository = pMeteorologicalRepository;
        }

        public async Task DownloadDataAsync(DownloadDataRequest request)
        {
            await DownloadHydrologicalForeCastAsync(request);
            await DownloadHydrologicalAsync(request);
            await DownloadRainQuantityAsync(request);
            await DownloadMeteorologicalAsync(request);
        }

        private async Task DownloadHydrologicalForeCastAsync(DownloadDataRequest request)
        {
            try
            {
                var listHydrologicalForecastOri =
                await _hydrologicalForecastRepository.GetByDateAsync(request.FromDate, request.ToDate);

                if(!listHydrologicalForecastOri.Any())
                    return;

                var listHydrologicalForecastDownload = new List<HydrologicalForeCast>();

                foreach (var hOri in listHydrologicalForecastOri)
                {
                    var hDownload =
                        listHydrologicalForecastDownload.FirstOrDefault(x => x.RefDate == hOri.RefDate
                                                                             && x.StationId == hOri.StationId);

                    var minValue = GetHydrologicalForeCastGetValue(hOri, 0, true);
                    var maxValue = GetHydrologicalForeCastGetValue(hOri, 0, false);
                    if (hDownload == null)
                    {
                        hDownload = new HydrologicalForeCast()
                        {
                            RefDate = hOri.RefDate,
                            StationId = hOri.StationId,
                            Id = Guid.NewGuid(),
                            Type = (int)HydrologicalForeCastType.Default
                        };

                        listHydrologicalForecastDownload.Add(hDownload);
                    }

                    if (minValue.HasValue)
                        hDownload.MinValue = minValue;
                    if (maxValue.HasValue)
                        hDownload.MaxValue = maxValue;

                    //Process for last date
                    if (hOri.RefDate.Date.Equals(request.ToDate.Date))
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            minValue = GetHydrologicalForeCastGetValue(hOri, i, true);
                            maxValue = GetHydrologicalForeCastGetValue(hOri, i, false);
                            var date = hOri.RefDate.AddDays(i);
                            var existingEntry =
                                listHydrologicalForecastDownload.FirstOrDefault(x => x.RefDate == date
                                                                                     && x.StationId == hOri.StationId);

                            if (existingEntry == null)
                            {
                                existingEntry = new HydrologicalForeCast()
                                {
                                    RefDate = date,
                                    StationId = hOri.StationId,
                                    Id = Guid.NewGuid(),
                                    Type = (int)HydrologicalForeCastType.Default
                                };

                                listHydrologicalForecastDownload.Add(existingEntry);
                            }

                            if (minValue.HasValue)
                                existingEntry.MinValue = minValue;
                            if (maxValue.HasValue)
                                existingEntry.MaxValue = maxValue;
                        }
                    }
                }

                await _pHydrologicalForecastRepository.DownloadDataAsync(listHydrologicalForecastDownload, request);
            }
            catch (Exception e)
            {
                //Log
                Console.WriteLine(e);
            }
        }

        private float? GetHydrologicalForeCastGetValue(MonitoringEntities.HydrologicalForecast entry, int index, bool min)
        {
            if ((min && entry.MinMax.ToUpper() == "MIN") || (!min && entry.MinMax.ToUpper() == "MAX"))
            {
                switch (index)
                {
                    case 0:
                        return entry.Day1;
                    case 1:
                        return entry.Day2;
                    case 2:
                        return entry.Day3;
                    case 3:
                        return entry.Day4;
                    case 4:
                        return entry.Day5;
                }
            }

            return null;
        }

        private async Task DownloadHydrologicalAsync(DownloadDataRequest request)
        {
            try
            {
                var fromDate = request.FromDate.GetStartOfDate();
                var toDate = request.ToDate.GetEndOfDate();
                var listHydrologicalOri =
                    await _hydrologicalRepository.GetByDateAsync(fromDate, toDate);

                if(!listHydrologicalOri.Any())
                    return;

                var listHydrologicalDownload =
                    _mapper.Map<List<Hydrological>>(listHydrologicalOri);

                await _pHydrologicalRepository.DownloadDataAsync(listHydrologicalDownload, request);
            }
            catch (Exception e)
            {
                //Log
                Console.WriteLine(e);
            }
        }

        private async Task DownloadRainQuantityAsync(DownloadDataRequest request)
        {
            try
            {
                var fromDate = request.FromDate.GetStartOfDate();
                var toDate = request.ToDate.GetEndOfDate();
                var listRainOri =
                    await _rainRepository.GetByDateAsync(fromDate, toDate);

                if (!listRainOri.Any())
                    return;

                var listRainQuantityDownload = _mapper.Map<List<RainQuantity>>(listRainOri);

                await _pRainQuantityRepository.DownloadDataAsync(listRainQuantityDownload, request);
            }
            catch (Exception e)
            {
                //Log
                Console.WriteLine(e);
            }
        }

        private async Task DownloadMeteorologicalAsync(DownloadDataRequest request)
        {
            try
            {
                var fromDate = request.FromDate.GetStartOfDate();
                var toDate = request.ToDate.GetEndOfDate();
                var listMeteorologicalOri =
                    await _meteorologicalRepository.GetByDateAsync(fromDate, toDate);

                if (!listMeteorologicalOri.Any())
                    return;

                var listMeteorologicalDownload = _mapper.Map<List<Meteorological>>(listMeteorologicalOri);

                await _pMeteorologicalRepository.DownloadDataAsync(listMeteorologicalDownload, request);
            }
            catch (Exception e)
            {
                //Log
                Console.WriteLine(e);
            }
        }

    }
}