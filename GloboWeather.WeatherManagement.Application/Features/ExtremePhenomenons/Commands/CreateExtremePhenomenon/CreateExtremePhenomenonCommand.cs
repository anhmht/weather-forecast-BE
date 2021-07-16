using System;
using System.Collections.Generic;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.CreateExtremePhenomenon
{
    public class CreateExtremePhenomenonCommand : IRequest<Guid>
    {
        public int ProvinceId { get; set; }
        public Guid DistrictId { get; set; }
        public DateTime Date { get; set; }
        public List<CreateExtremePhenomenonDetailDto> Details { get; set; }
    }
}
