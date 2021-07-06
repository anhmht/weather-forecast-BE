using GloboWeather.WeatherManagement.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManagement.Application.Features.Hydrologicals.Import
{
    public class ImportHydrologicalCommand : IRequest<ImportResponse>
    {
        public IFormFile File { get; set; }
    }
}
