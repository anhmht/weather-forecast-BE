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

namespace GloboWeather.WeatherManagement.Application.Features.HydrologicalForeCasts.Import
{
    public class ImportHydrologicalForeCastCommandHandle : IRequestHandler<ImportHydrologicalForeCastCommand, ImportResponse>
    {
        private readonly IMapper _mapper;
        private readonly IHydrologicalForeCastRepository _hydrologicalForeCastRepository;
        public ImportHydrologicalForeCastCommandHandle(IMapper mapper, IHydrologicalForeCastRepository hydrologicalForeCastRepository)
        {
            _mapper = mapper;
            _hydrologicalForeCastRepository = hydrologicalForeCastRepository;
        }

        public async Task<ImportResponse> Handle(ImportHydrologicalForeCastCommand request, CancellationToken cancellationToken)
        {
            var response = new ImportResponse() { Success = true };

            if (!request.File.ContentType.Contains("ms-excel"))
            {
                response.Success = false;
                response.Message = "Please use the 'CSV' file to import.";
                return response;
            }

            var validatorDto = new ImportHydrologicalForeCastDtoValidator();
            using (var reader = request.File.OpenReadStream())
            using (var streamReader = new StreamReader(reader))
            using (var csv = new CsvReader(streamReader))
            {
                var hydrologycalForeCasts = csv.GetRecords<ImportHydrologicalForeCastDto>().ToList();

                var errorItems = new List<RowError>();
                for (int i = 0; i < hydrologycalForeCasts.Count; i++)
                {
                    var validationResult = await validatorDto.ValidateAsync(hydrologycalForeCasts[i], cancellationToken);

                    if (validationResult.Errors.Any())
                    {
                        errorItems.Add(new RowError { RowIndex = i + 2, ErrorMessage = validationResult.Errors.Select(x => x.ErrorMessage) });
                    }

                    if (hydrologycalForeCasts.Count(x => x.RefDate == hydrologycalForeCasts[i].RefDate
                                                 && x.StationId == hydrologycalForeCasts[i].StationId) > 1)
                    {
                        errorItems.Add(new RowError { RowIndex = i + 2, ErrorMessage = new[] { $"{hydrologycalForeCasts[i].RefDate} currently has more than one record. Please keep only one record and delete the others." } });
                    }

                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].MinValue))
                        hydrologycalForeCasts[i].MinValue = null;
                    if (string.IsNullOrEmpty(hydrologycalForeCasts[i].MaxValue))
                        hydrologycalForeCasts[i].MaxValue = null;
                }

                if (errorItems.Count > 0)
                {
                    response.Success = false;
                    response.Message = "The data content is invalid.";
                    response.RowErrors = errorItems;
                    return response;
                }
                
                var importData = _mapper.Map<List<HydrologicalForeCast>>(hydrologycalForeCasts);
                await _hydrologicalForeCastRepository.ImportAsync(request, importData, cancellationToken);
            }

            return response;
        }
    }
}
