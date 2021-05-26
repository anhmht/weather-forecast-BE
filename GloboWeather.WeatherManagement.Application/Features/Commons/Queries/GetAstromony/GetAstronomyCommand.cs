using MediatR;

namespace GloboWeather.WeatherManagement.Application.Models.Astronomy
{
    public class GetAstronomyCommand
    {
        public  decimal Lat { get; set; }
        public  decimal Lon { get; set; }
    }
}