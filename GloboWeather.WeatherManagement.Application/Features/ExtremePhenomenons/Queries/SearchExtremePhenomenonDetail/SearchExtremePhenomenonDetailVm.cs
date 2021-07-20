using System.Collections.Generic;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.SearchExtremePhenomenonDetail
{
    public class SearchExtremePhenomenonDetailVm : ExtremePhenomenon
    {
        public List<Domain.Entities.ExtremePhenomenonDetail> Details { get; set; }
    }
}