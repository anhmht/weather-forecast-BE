using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformationHorizontal;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation
{
    public class GetWeatherInformationHorizontalHandler : IRequestHandler<GetWeatherInformationHorizontalRequest, GetWeatherInformationHorizontalResponse>
    {
        private readonly IWeatherInformationService _weatherInformationService;

        public GetWeatherInformationHorizontalHandler(IWeatherInformationService weatherInformationService)
        {
            _weatherInformationService = weatherInformationService;
        }

        public async Task<GetWeatherInformationHorizontalResponse> Handle(GetWeatherInformationHorizontalRequest request, CancellationToken cancellationToken)
        {
            var validator = new GetWeatherInformationBaseRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            var response = await _weatherInformationService.GetWeatherInformationHorizontalAsync(request, cancellationToken);

            return response;
        }
    }
}
