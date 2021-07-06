using GloboWeather.WeatherManagement.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManagement.Application.Features.Meteorologicals.Import
{
    public class ImportMeteorologicalCommand : IRequest<ImportResponse>
    {
        public IFormFile File { get; set; }
    }
}
