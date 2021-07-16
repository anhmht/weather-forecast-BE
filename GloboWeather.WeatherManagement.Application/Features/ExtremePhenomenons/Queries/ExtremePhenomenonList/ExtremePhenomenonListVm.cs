using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.ExtremePhenomenonList
{
    public class ExtremePhenomenonListVm : ExtremePhenomenon
    {
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
    }
}