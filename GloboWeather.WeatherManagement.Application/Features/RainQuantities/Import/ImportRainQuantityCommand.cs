using GloboWeather.WeatherManagement.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManagement.Application.Features.RainQuantities.Import
{
    public class ImportRainQuantityCommand : IRequest<ImportResponse>
    {
        public IFormFile File { get; set; }
    }
}
