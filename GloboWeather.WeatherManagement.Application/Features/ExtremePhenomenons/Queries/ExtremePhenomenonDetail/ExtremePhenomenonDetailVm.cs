using System.Collections.Generic;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.ExtremePhenomenonDetail
{
    public class ExtremePhenomenonDetailVm : ExtremePhenomenon
    {
        public List<Domain.Entities.ExtremePhenomenonDetail> Details { get; set; }
    }
}