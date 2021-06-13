using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListByCateIdAndStaId
{
    public class GetMinMaxHumidityHandler : IRequestHandler<GetMinMaxHumidityRequest, HumidityPredictionResponse>
    {
        private readonly IWeatherInformationService _weatherInformationService;

        public GetMinMaxHumidityHandler(IMapper mapper, IWeatherInformationService weatherInformationService)
        {
            _weatherInformationService = weatherInformationService;
        }

        public async Task<HumidityPredictionResponse> Handle(GetMinMaxHumidityRequest request, CancellationToken cancellationToken)
        {
            var validator = new WeatherInformationBaseRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            var response = await _weatherInformationService.GetHumidityAsync(request.StationId, request.FromDate, request.ToDate, cancellationToken);

            return response;
        }
    }
}