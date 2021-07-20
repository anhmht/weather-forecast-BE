using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.SearchExtremePhenomenonDetail
{
    public class SearchExtremePhenomenonDetailQuery: IRequest<SearchExtremePhenomenonDetailVm>
    {
        public int ProvinceId { get; set; }
        public Guid DistrictId { get; set; }
        public DateTime? Date { get; set; }
    }
}