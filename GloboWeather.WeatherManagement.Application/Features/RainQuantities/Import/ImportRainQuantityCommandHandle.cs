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

namespace GloboWeather.WeatherManagement.Application.Features.RainQuantities.Import
{
    public class ImportRainQuantityCommandHandle : IRequestHandler<ImportRainQuantityCommand, ImportResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRainQuantityRepository _rainQuantityRepository;
        public ImportRainQuantityCommandHandle(IMapper mapper, IRainQuantityRepository rainQuantityRepository)
        {
            _mapper = mapper;
            _rainQuantityRepository = rainQuantityRepository;
        }

        public async Task<ImportResponse> Handle(ImportRainQuantityCommand request, CancellationToken cancellationToken)
        {
            var response = new ImportResponse() { Success = true };

            if (!request.File.ContentType.Contains("ms-excel"))
            {
                response.Success = false;
                response.Message = "Please use the 'CSV' file to import.";
                return response;
            }

            var validatorDto = new ImportRainQuantityDtoValidator();
            using (var reader = request.File.OpenReadStream())
            using (var streamReader = new StreamReader(reader))
            using (var csv = new CsvReader(streamReader))
            {
                var rainQuantitiesDtos = csv.GetRecords<ImportRainQuantityDto>().ToList();

                var errorItems = new List<RowError>();
                for (int i = 0; i < rainQuantitiesDtos.Count; i++)
                {
                    var validationResult = await validatorDto.ValidateAsync(rainQuantitiesDtos[i], cancellationToken);

                    if (validationResult.Errors.Any())
                    {
                        errorItems.Add(new RowError { RowIndex = i + 2, ErrorMessage = validationResult.Errors.Select(x => x.ErrorMessage) });
                    }

                    if (rainQuantitiesDtos.Count(x => x.RefDate == rainQuantitiesDtos[i].RefDate
                                                 && x.StationId == rainQuantitiesDtos[i].StationId) > 1)
                    {
                        errorItems.Add(new RowError { RowIndex = i + 2, ErrorMessage = new[] { $"{rainQuantitiesDtos[i].RefDate} currently has more than one record. Please keep only one record and delete the others." } });
                    }

                    if (string.IsNullOrEmpty(rainQuantitiesDtos[i].Value))
                        rainQuantitiesDtos[i].Value = null;
                }

                if (errorItems.Count > 0)
                {
                    response.Success = false;
                    response.Message = "The data content is invalid.";
                    response.RowErrors = errorItems;
                    return response;
                }
                
                var importData = _mapper.Map<List<RainQuantity>>(rainQuantitiesDtos);
                await _rainQuantityRepository.ImportAsync(request, importData, cancellationToken);
            }

            return response;
        }
    }
}
