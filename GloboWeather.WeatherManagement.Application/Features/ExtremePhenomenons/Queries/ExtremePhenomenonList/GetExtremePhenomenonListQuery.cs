using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.ExtremePhenomenonList
{
    public class GetExtremePhenomenonListQuery : IRequest<GetExtremePhenomenonListResponse>
    {
        public  int Limit { get; set; }
        public int Page { get; set; }
        public DateTime? Date { get; set; }
        public int? ProvinceId { get; set; }

    }
}