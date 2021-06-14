using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation
{
    public class GetWeatherInformationHandler : IRequestHandler<GetWeatherInformationRequest, GetWeatherInformationResponse>
    {
        private readonly IWeatherInformationService _weatherInformationService;

        public GetWeatherInformationHandler(IWeatherInformationService weatherInformationService)
        {
            _weatherInformationService = weatherInformationService;
        }

        public async Task<GetWeatherInformationResponse> Handle(GetWeatherInformationRequest request, CancellationToken cancellationToken)
        {
            var validator = new GetWeatherInformationRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            var response = await _weatherInformationService.GetWeatherInformationsAsync(request, cancellationToken);

            return response;
        }
    }
}
