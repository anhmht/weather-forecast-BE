using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportSingleStation;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInformation
{
    public class ImportWeatherInformationDto : ImportSingleStationDto
    {
        public string DiaDiemId { get; set; }
    }
}