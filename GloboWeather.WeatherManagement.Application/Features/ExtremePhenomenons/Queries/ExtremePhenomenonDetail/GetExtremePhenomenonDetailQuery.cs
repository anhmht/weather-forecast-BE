using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.ExtremePhenomenonDetail
{
    public class GetExtremePhenomenonDetailQuery: IRequest<ExtremePhenomenonDetailVm>
    {
        public Guid Id { get; set; }
    }
}