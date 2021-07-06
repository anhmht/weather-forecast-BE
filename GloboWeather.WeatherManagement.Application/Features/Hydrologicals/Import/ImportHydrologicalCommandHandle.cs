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

namespace GloboWeather.WeatherManagement.Application.Features.Hydrologicals.Import
{
    public class ImportHydrologicalCommandHandle : IRequestHandler<ImportHydrologicalCommand, ImportResponse>
    {
        private readonly IMapper _mapper;
        private readonly IHydrologicalRepository _hydrologicalRepository;
        public ImportHydrologicalCommandHandle(IMapper mapper, IHydrologicalRepository hydrologicalRepository)
        {
            _mapper = mapper;
            _hydrologicalRepository = hydrologicalRepository;
        }

        public async Task<ImportResponse> Handle(ImportHydrologicalCommand request, CancellationToken cancellationToken)
        {
            var response = new ImportResponse() { Success = true };

            if (!request.File.ContentType.Contains("ms-excel"))
            {
                response.Success = false;
                response.Message = "Please use the 'CSV' file to import.";
                return response;
            }

            var validatorDto = new ImportHydrologicalDtoValidator();
            using (var reader = request.File.OpenReadStream())
            using (var streamReader = new StreamReader(reader))
            using (var csv = new CsvReader(streamReader))
            {
                var hydrologycals = csv.GetRecords<ImportHydrologicalDto>().ToList();

                var errorItems = new List<RowError>();
                for (int i = 0; i < hydrologycals.Count; i++)
                {
                    var validationResult = await validatorDto.ValidateAsync(hydrologycals[i], cancellationToken);

                    if (validationResult.Errors.Any())
                    {
                        errorItems.Add(new RowError { RowIndex = i + 2, ErrorMessage = validationResult.Errors.Select(x => x.ErrorMessage) });
                    }

                    if (hydrologycals.Count(x => x.Date == hydrologycals[i].Date
                                                 && x.StationId == hydrologycals[i].StationId) > 1)
                    {
                        errorItems.Add(new RowError { RowIndex = i + 2, ErrorMessage = new[] { $"{hydrologycals[i].Date} currently has more than one record. Please keep only one record and delete the others." } });
                    }

                    if (string.IsNullOrEmpty(hydrologycals[i].Accumulated))
                        hydrologycals[i].Accumulated = null;
                    if (string.IsNullOrEmpty(hydrologycals[i].Rain))
                        hydrologycals[i].Rain = null;
                    if (string.IsNullOrEmpty(hydrologycals[i].WaterLevel))
                        hydrologycals[i].WaterLevel = null;
                }

                if (errorItems.Count > 0)
                {
                    response.Success = false;
                    response.Message = "The data content is invalid.";
                    response.RowErrors = errorItems;
                    return response;
                }
                
                var importData = _mapper.Map<List<Hydrological>>(hydrologycals);
                await _hydrologicalRepository.ImportAsync(request, importData, cancellationToken);
            }

            return response;
        }
    }
}
