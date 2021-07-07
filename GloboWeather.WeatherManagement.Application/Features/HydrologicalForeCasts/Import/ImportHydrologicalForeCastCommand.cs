using GloboWeather.WeatherManagement.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManagement.Application.Features.HydrologicalForeCasts.Import
{
    public class ImportHydrologicalForeCastCommand : IRequest<ImportResponse>
    {
        public IFormFile File { get; set; }
    }
}
