using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Features.Meteorologicals.Import;
using GloboWeather.WeatherManagement.Application.Requests;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Repositories
{
    public class MeteorologicalRepository : BaseRepository<Meteorological>, IMeteorologicalRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public MeteorologicalRepository(GloboWeatherDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> DownloadDataAsync(List<Meteorological> meteorologicals, DownloadDataRequest request)
        {
            var fromDate = meteorologicals.Min(x => x.Date);
            var toDate = meteorologicals.Max(x => x.Date);
            var existingMeteorologicals =
                await _unitOfWork.MeteorologicalRepository.GetWhereAsync(x => x.Date >= fromDate
                                                                                    && x.Date <= toDate);

            foreach (var itemDownloaded in meteorologicals)
            {
                var entry =
                    existingMeteorologicals.FirstOrDefault(x => x.StationId.Equals(itemDownloaded.StationId)
                                                                      && x.Date.Equals(itemDownloaded.Date));

                var windSpeed = itemDownloaded.WindSpeed == -9990 || itemDownloaded.WindSpeed == -9999
                    ? null
                    : itemDownloaded.WindSpeed;
                var barometric = itemDownloaded.Barometric == -9990 || itemDownloaded.Barometric == -9999
                    ? null
                    : itemDownloaded.Barometric;
                var evaporation = itemDownloaded.Evaporation == -9990 || itemDownloaded.Evaporation == -9999
                    ? null
                    : itemDownloaded.Evaporation;
                var hga10 = itemDownloaded.Hga10 == -9990 || itemDownloaded.Hga10 == -9999
                    ? null
                    : itemDownloaded.Hga10;
                var hgm60 = itemDownloaded.Hgm60 == -9990 || itemDownloaded.Hgm60 == -9999
                    ? null
                    : itemDownloaded.Hgm60;
                var humidity = itemDownloaded.Humidity == -9990 || itemDownloaded.Humidity == -9999
                    ? null
                    : itemDownloaded.Humidity;
                var radiation = itemDownloaded.Radiation == -9990 || itemDownloaded.Radiation == -9999
                    ? null
                    : itemDownloaded.Radiation;
                var rain = itemDownloaded.Rain == -9990 || itemDownloaded.Rain == -9999
                    ? null
                    : itemDownloaded.Rain;
                var sunnyTime = itemDownloaded.SunnyTime == -9990 || itemDownloaded.SunnyTime == -9999
                    ? null
                    : itemDownloaded.SunnyTime;
                var tdga10 = itemDownloaded.Tdga10 == -9990 || itemDownloaded.Tdga10 == -9999
                    ? null
                    : itemDownloaded.Tdga10;
                var tdgm60 = itemDownloaded.Tdgm60 == -9990 || itemDownloaded.Tdgm60 == -9999
                    ? null
                    : itemDownloaded.Tdgm60;
                var temperature = itemDownloaded.Temperature == -9990 || itemDownloaded.Temperature == -9999
                    ? null
                    : itemDownloaded.Temperature;
                var windDirection = itemDownloaded.WindDirection == -9990 || itemDownloaded.WindDirection == -9999
                    ? null
                    : itemDownloaded.WindDirection;
                var zluyKe = itemDownloaded.ZluyKe == -9990 || itemDownloaded.ZluyKe == -9999
                    ? null
                    : itemDownloaded.ZluyKe;
                var isUpdate = false;

                if (entry != null)
                {
                    if (entry.WindSpeed != windSpeed)
                    {
                        entry.WindSpeed = windSpeed;
                        isUpdate = true;
                    }
                    if (entry.Barometric != barometric)
                    {
                        entry.Barometric = barometric;
                        isUpdate = true;
                    }
                    if (entry.Evaporation != evaporation)
                    {
                        entry.Evaporation = evaporation;
                        isUpdate = true;
                    }
                    if (entry.Hga10 != hga10)
                    {
                        entry.Hga10 = hga10;
                        isUpdate = true;
                    }
                    if (entry.Hgm60 != hgm60)
                    {
                        entry.Hgm60 = hgm60;
                        isUpdate = true;
                    }
                    if (entry.Humidity != humidity)
                    {
                        entry.Humidity = humidity;
                        isUpdate = true;
                    }
                    if (entry.Radiation != radiation)
                    {
                        entry.Radiation = radiation;
                        isUpdate = true;
                    }
                    if (entry.Rain != rain)
                    {
                        entry.Rain = rain;
                        isUpdate = true;
                    }
                    if (entry.SunnyTime != sunnyTime)
                    {
                        entry.SunnyTime = sunnyTime;
                        isUpdate = true;
                    }
                    if (entry.Tdga10 != tdga10)
                    {
                        entry.Tdga10 = tdga10;
                        isUpdate = true;
                    }
                    if (entry.Tdgm60 != tdgm60)
                    {
                        entry.Tdgm60 = tdgm60;
                        isUpdate = true;
                    }
                    if (entry.Temperature != temperature)
                    {
                        entry.Temperature = temperature;
                        isUpdate = true;
                    }
                    if (entry.WindDirection != windDirection)
                    {
                        entry.WindDirection = windDirection;
                        isUpdate = true;
                    }
                    if (entry.ZluyKe != zluyKe)
                    {
                        entry.ZluyKe = zluyKe;
                        isUpdate = true;
                    }

                    if (isUpdate)
                    {
                        _unitOfWork.MeteorologicalRepository.Update(entry);
                    }
                }
                else
                {
                    entry = new Meteorological()
                    {
                        Id = Guid.NewGuid(),
                        StationId = itemDownloaded.StationId,
                        Date = itemDownloaded.Date,
                        WindSpeed = windSpeed,
                        Barometric = barometric,
                        Evaporation = evaporation,
                        Hga10 = hga10,
                        Hgm60 = hgm60,
                        Humidity = humidity,
                        Radiation = radiation,
                        Rain = rain,
                        SunnyTime = sunnyTime,
                        Tdga10 = tdga10,
                        Tdgm60 = tdgm60,
                        Temperature = temperature,
                        WindDirection = windDirection,
                        ZluyKe = zluyKe
                    };
                    _unitOfWork.MeteorologicalRepository.Add(entry);
                }
            }

            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> ImportAsync(ImportMeteorologicalCommand request, List<Meteorological> meteorologicals, CancellationToken cancellationToken)
        {
            var fromDate = meteorologicals.Min(x => x.Date);
            var toDate = meteorologicals.Max(x => x.Date);
            var listStation = meteorologicals.Select(x => x.StationId).Distinct();

            var existingItems = await _unitOfWork.MeteorologicalRepository
                .GetWhereAsync(x => listStation.Contains(x.StationId)
                                    && x.Date >= fromDate && x.Date <= toDate, cancellationToken);

            foreach (var entry in meteorologicals)
            {
                var meteorological =
                    existingItems.FirstOrDefault(x => x.StationId == entry.StationId && x.Date == entry.Date);
                if (meteorological == null)
                {
                    meteorological = new Meteorological()
                    {
                        Date = entry.Date,
                        Id = Guid.NewGuid(),
                        StationId = entry.StationId,
                        WindSpeed = entry.WindSpeed,
                        Hga10 = entry.Hga10,
                        Humidity = entry.Humidity,
                        Evaporation = entry.Evaporation,
                        Radiation = entry.Radiation,
                        Barometric = entry.Barometric,
                        Tdga10 = entry.Tdga10,
                        Tdgm60 = entry.Tdgm60,
                        Temperature = entry.Temperature,
                        WindDirection = entry.WindDirection,
                        SunnyTime = entry.SunnyTime,
                        Hgm60 = entry.Hgm60,
                        ZluyKe = entry.ZluyKe,
                        Rain = entry.Rain
                    };
                    _unitOfWork.MeteorologicalRepository.Add(meteorological);
                }
                else
                {
                    var isUpdate = false;
                    if (!meteorological.WindSpeed.Equals(entry.WindSpeed))
                    {
                        meteorological.WindSpeed = entry.WindSpeed;
                        isUpdate = true;
                    }
                    if (!meteorological.Hga10.Equals(entry.Hga10))
                    {
                        meteorological.Hga10 = entry.Hga10;
                        isUpdate = true;
                    }
                    if (!meteorological.Humidity.Equals(entry.Humidity))
                    {
                        meteorological.Humidity = entry.Humidity;
                        isUpdate = true;
                    }
                    if (!meteorological.Evaporation.Equals(entry.Evaporation))
                    {
                        meteorological.Evaporation = entry.Evaporation;
                        isUpdate = true;
                    }
                    if (!meteorological.Radiation.Equals(entry.Radiation))
                    {
                        meteorological.Radiation = entry.Radiation;
                        isUpdate = true;
                    }
                    if (!meteorological.Barometric.Equals(entry.Barometric))
                    {
                        meteorological.Barometric = entry.Barometric;
                        isUpdate = true;
                    }
                    if (!meteorological.Tdga10.Equals(entry.Tdga10))
                    {
                        meteorological.Tdga10 = entry.Tdga10;
                        isUpdate = true;
                    }
                    if (!meteorological.Tdgm60.Equals(entry.Tdgm60))
                    {
                        meteorological.Tdgm60 = entry.Tdgm60;
                        isUpdate = true;
                    }
                    if (!meteorological.Temperature.Equals(entry.Temperature))
                    {
                        meteorological.Temperature = entry.Temperature;
                        isUpdate = true;
                    }
                    if (!meteorological.WindDirection.Equals(entry.WindDirection))
                    {
                        meteorological.WindDirection = entry.WindDirection;
                        isUpdate = true;
                    }
                    if (!meteorological.SunnyTime.Equals(entry.SunnyTime))
                    {
                        meteorological.SunnyTime = entry.SunnyTime;
                        isUpdate = true;
                    }
                    if (!meteorological.Hgm60.Equals(entry.Hgm60))
                    {
                        meteorological.Hgm60 = entry.Hgm60;
                        isUpdate = true;
                    }
                    if (!meteorological.ZluyKe.Equals(entry.ZluyKe))
                    {
                        meteorological.ZluyKe = entry.ZluyKe;
                        isUpdate = true;
                    }
                    if (!meteorological.Rain.Equals(entry.Rain))
                    {
                        meteorological.Rain = entry.Rain;
                        isUpdate = true;
                    }

                    if (isUpdate)
                    {
                        _unitOfWork.MeteorologicalRepository.Update(meteorological);
                    }
                }
            }

            return await _unitOfWork.CommitAsync();
        }
    }
}