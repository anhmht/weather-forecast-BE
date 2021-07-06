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

namespace GloboWeather.WeatherManagement.Application.Features.Meteorologicals.Import
{
    public class ImportMeteorologicalCommandHandle : IRequestHandler<ImportMeteorologicalCommand, ImportResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMeteorologicalRepository _meteorologicalRepository;
        public ImportMeteorologicalCommandHandle(IMapper mapper, IMeteorologicalRepository meteorologicalRepository)
        {
            _mapper = mapper;
            _meteorologicalRepository = meteorologicalRepository;
        }

        public async Task<ImportResponse> Handle(ImportMeteorologicalCommand request, CancellationToken cancellationToken)
        {
            var response = new ImportResponse() { Success = true };

            if (!request.File.ContentType.Contains("ms-excel"))
            {
                response.Success = false;
                response.Message = "Please use the 'CSV' file to import.";
                return response;
            }

            var validatorDto = new ImportMeteorologicalDtoValidator();
            using (var reader = request.File.OpenReadStream())
            using (var streamReader = new StreamReader(reader))
            using (var csv = new CsvReader(streamReader))
            {
                var hydrologycalForeCasts = csv.GetRecords<ImportMeteorologicalDto>().ToList();

                var errorItems = new List<RowError>();
                for (int i = 0; i < hydrologycalForeCasts.Count; i++)
                {
                    var validationResult = await validatorDto.ValidateAsync(hydrologycalForeCasts[i], cancellationToken);

                    if (validationResult.Errors.Any())
                    {
                        errorItems.Add(new RowError { RowIndex = i + 2, ErrorMessage = validationResult.Errors.Select(x => x.ErrorMessage) });
                    }

                    if (hydrologycalForeCasts.Count(x => x.Date == hydrologycalForeCasts[i].Date
                                                 && x.StationId == hydrologycalForeCasts[i].StationId) > 1)
                    {
                        errorItems.Add(new RowError { RowIndex = i + 2, ErrorMessage = new[] { $"{hydrologycalForeCasts[i].Date} currently has more than one record. Please keep only one record and delete the others." } });
                    }

                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].Evaporation))
                        hydrologycalForeCasts[i].Evaporation = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].Radiation))
                        hydrologycalForeCasts[i].Radiation = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].Humidity))
                        hydrologycalForeCasts[i].Humidity = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].WindDirection))
                        hydrologycalForeCasts[i].WindDirection = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].Barometric))
                        hydrologycalForeCasts[i].Barometric = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].Hga10))
                        hydrologycalForeCasts[i].Hga10 = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].Hgm60))
                        hydrologycalForeCasts[i].Hgm60 = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].Rain))
                        hydrologycalForeCasts[i].Rain = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].Temperature))
                        hydrologycalForeCasts[i].Temperature = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].Tdga10))
                        hydrologycalForeCasts[i].Tdga10 = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].Tdgm60))
                        hydrologycalForeCasts[i].Tdgm60 = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].WindSpeed))
                        hydrologycalForeCasts[i].WindSpeed = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].SunnyTime))
                        hydrologycalForeCasts[i].SunnyTime = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].ZluyKe))
                        hydrologycalForeCasts[i].ZluyKe = null;
                }

                if (errorItems.Count > 0)
                {
                    response.Success = false;
                    response.Message = "The data content is invalid.";
                    response.RowErrors = errorItems;
                    return response;
                }
                
                var importData = _mapper.Map<List<Meteorological>>(hydrologycalForeCasts);
                await _meteorologicalRepository.ImportAsync(request, importData, cancellationToken);
            }

            return response;
        }
    }
}
