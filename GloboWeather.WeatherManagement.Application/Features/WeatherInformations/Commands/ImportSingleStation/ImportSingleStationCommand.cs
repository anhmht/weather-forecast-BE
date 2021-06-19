using MediatR;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportSingleStation
{
    public class ImportSingleStationCommand : IRequest<ImportSingleStationResponse>
    {
        public string StationId { get; set; }
        public string StationName { get; set; }
        public IFormFile File { get; set; }
    }
}