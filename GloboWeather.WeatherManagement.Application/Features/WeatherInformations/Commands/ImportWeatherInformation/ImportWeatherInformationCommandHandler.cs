using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInformation;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInfomation
{
    public class ImportWeatherInformationCommandHandler : IRequestHandler<ImportWeatherInformationCommand, ImportWeatherInformationResponse>
    {

        private readonly IMapper _mapper;
        private readonly IWeatherInformationRepository _weatherInfomationRepository;

        public ImportWeatherInformationCommandHandler(IMapper mapper, IWeatherInformationRepository weatherInfomationRepository)
        {
            _mapper = mapper;
            _weatherInfomationRepository = weatherInfomationRepository;
        }

        public async Task<ImportWeatherInformationResponse> Handle(ImportWeatherInformationCommand request, CancellationToken token)
        {
            var validator = new ImportWeatherInformationValidator(_weatherInfomationRepository);
            var response = new ImportWeatherInformationResponse() { Success = true };
            if (!request.File.ContentType.Contains("ms-excel"))
            {
                response.Success = false;
                response.Message = "Please use the 'CSV' file to import.";
                return response;
            }

            using (var reader = request.File.OpenReadStream())
            using (var streamReader = new StreamReader(reader))
            using (var csv = new CsvReader(streamReader))
            {
                var weatherInformations = csv.GetRecords<ImportWeatherInformationDto>().ToList();

                var errorItems = new List<RowError>();
                for (int i = 0; i < weatherInformations.Count; i++)
                {
                    var validationResult = await validator.ValidateAsync(weatherInformations[i]);

                    if (validationResult.Errors.Any())
                    {
                        errorItems.Add(new RowError() { RowIndex = i + 2, ErrorMessage = validationResult.Errors.Select(x => x.ErrorMessage) });
                    }
                }

                if (errorItems.Count > 0)
                {
                    response.Success = false;
                    response.Message = "The data content is invalid.";
                    response.RowErrors = errorItems;
                    return response;
                }

                var importData = _mapper.Map<List<WeatherInformation>>(weatherInformations);
                var maxRefDate = importData.Max(x => x.RefDate);
                var minRefDate = importData.Min(x => x.RefDate);
                var stationIds = importData.Select(x => x.StationId).Distinct().ToList();
                var existingData = await _weatherInfomationRepository.GetByRefDateStationAsync(minRefDate, maxRefDate, stationIds, token);

                var listInsert = (from i in importData
                                  join e in existingData on new { i.StationId, i.RefDate } equals new { e.StationId, e.RefDate }
                                  into t
                                  from vkl in t.DefaultIfEmpty()
                                  where vkl == null
                                  select new WeatherInformation()
                                  {
                                      //CreateBy = "import",
                                      //CreateDate = DateTime.Now,
                                      Humidity = i.Humidity,
                                      ID = Guid.NewGuid(),
                                      //LastModifiedBy = "import",
                                      //LastModifiedDate = DateTime.Now,
                                      RainAmount = i.RainAmount,
                                      RefDate = i.RefDate,
                                      StationId = i.StationId,
                                      Temperature = i.Temperature,
                                      Weather = i.Weather,
                                      WindDirection = i.WindDirection,
                                      WindLevel = i.WindLevel,
                                      WindSpeed = i.WindSpeed
                                  }).ToList();

                if (listInsert.Count > 0)
                    await _weatherInfomationRepository.AddRangeAsync(listInsert);

                if (existingData.Any())
                {
                    foreach (var item in existingData)
                    {
                        var updateItem = importData.FirstOrDefault(x => x.StationId == item.StationId && x.RefDate == item.RefDate);
                        if (updateItem != null)
                        {
                            if (!string.IsNullOrWhiteSpace(updateItem.Humidity)) //Don't update if don't input value for this field
                                item.Humidity = updateItem.Humidity;
                            if (!string.IsNullOrWhiteSpace(updateItem.RainAmount)) //Don't update if don't input value for this field
                                item.RainAmount = updateItem.RainAmount;
                            if (!string.IsNullOrWhiteSpace(updateItem.Temperature)) //Don't update if don't input value for this field
                                item.Temperature = updateItem.Temperature;
                            if (!string.IsNullOrWhiteSpace(updateItem.Weather)) //Don't update if don't input value for this field
                                item.Weather = updateItem.Weather;
                            if (!string.IsNullOrWhiteSpace(updateItem.WindDirection)) //Don't update if don't input value for this field
                                item.WindDirection = updateItem.WindDirection;
                            if (!string.IsNullOrWhiteSpace(updateItem.WindLevel)) //Don't update if don't input value for this field
                                item.WindLevel = updateItem.WindLevel;
                            if (!string.IsNullOrWhiteSpace(updateItem.WindSpeed)) //Don't update if don't input value for this field
                                item.WindSpeed = updateItem.WindSpeed;
                        }
                    }

                    await _weatherInfomationRepository.UpdateRangeAsync(existingData.ToList());
                }
            }

            return response;
        }

    }
}