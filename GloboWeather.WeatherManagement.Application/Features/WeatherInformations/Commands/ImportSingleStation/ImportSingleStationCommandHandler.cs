using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Responses;
using GloboWeather.WeatherManagement.Domain.Entities;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportSingleStation
{
    public class ImportSingleStationCommandHandler : IRequestHandler<ImportSingleStationCommand, ImportSingleStationResponse>
    {

        private readonly IMapper _mapper;
        private readonly IWeatherInformationRepository _weatherInfomationRepository;

        public ImportSingleStationCommandHandler(IMapper mapper, IWeatherInformationRepository weatherInfomationRepository)
        {
            _mapper = mapper;
            _weatherInfomationRepository = weatherInfomationRepository;
        }

        public async Task<ImportSingleStationResponse> Handle(ImportSingleStationCommand request, CancellationToken token)
        {
            var validatorCommand = new ImportSingleStationCommandValidator(_weatherInfomationRepository);
            var validatorDto = new ImportSingleStationDtoValidator(_weatherInfomationRepository);
            var response = new ImportSingleStationResponse() { Success = true };

            var validationCommandResult = await validatorCommand.ValidateAsync(request);

            if (validationCommandResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationCommandResult);
            }

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
                var weatherInformations = csv.GetRecords<ImportSingleStationDto>().ToList();

                var errorItems = new List<RowError>();
                for (int i = 0; i < weatherInformations.Count; i++)
                {
                    var validationResult = await validatorDto.ValidateAsync(weatherInformations[i]);

                    if (validationResult.Errors.Any())
                    {
                        errorItems.Add(new RowError { RowIndex = i + 2, ErrorMessage = validationResult.Errors.Select(x => x.ErrorMessage) });
                    }

                    if (weatherInformations.Count(x => x.NgayGio == weatherInformations[i].NgayGio) > 1)
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
                response.Data = await _weatherInfomationRepository.ImportSingleStationAsync(request.StationId, request.StationName, importData, token);
            }

            return response;
        }

    }
}