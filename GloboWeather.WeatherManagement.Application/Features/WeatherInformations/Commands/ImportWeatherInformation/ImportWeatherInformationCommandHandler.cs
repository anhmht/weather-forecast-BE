using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInformation;
using GloboWeather.WeatherManagement.Application.Responses;
using GloboWeather.WeatherManagement.Domain.Entities;
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

                    if (weatherInformations.Count(x => x.NgayGio == weatherInformations[i].NgayGio
                                                       && x.DiaDiemId == weatherInformations[i].DiaDiemId) > 1)
                    {
                        errorItems.Add(new RowError { RowIndex = i + 2, ErrorMessage = new[] { $"{weatherInformations[i].NgayGio} currently has more than one record. Please keep only one record and delete the others." } });
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
                await _weatherInfomationRepository.ImportAsync(importData, token);
            }

            return response;
        }

    }
}