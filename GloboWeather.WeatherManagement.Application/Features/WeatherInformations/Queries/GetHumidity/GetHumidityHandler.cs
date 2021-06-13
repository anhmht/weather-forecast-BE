using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListByCateIdAndStaId
{
    public class GetHumidityHandler : IRequestHandler<GetHumidityRequest, HumidityResponse>
    {
        private readonly IWeatherInformationService _weatherInformationService;

        public GetHumidityHandler(IMapper mapper, IWeatherInformationService weatherInformationService)
        {
            _weatherInformationService = weatherInformationService;
        }

        public async Task<HumidityResponse> Handle(GetHumidityRequest request, CancellationToken cancellationToken)
        {
            var validator = new WeatherInformationBaseRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            var response = await _weatherInformationService.GetWeatherInformationVerticalAsync<HumidityResponse>(request.StationId, request.FromDate, request.ToDate, Helpers.Common.WeatherType.Humidity, cancellationToken);

            return response;
        }
    }
}