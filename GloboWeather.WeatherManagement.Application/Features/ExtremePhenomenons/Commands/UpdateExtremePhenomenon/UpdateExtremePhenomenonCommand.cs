using System;
using System.Collections.Generic;
using GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.CreateExtremePhenomenon;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.UpdateExtremePhenomenon
{
    public class UpdateExtremePhenomenonCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public int ProvinceId { get; set; }
        public Guid DistrictId { get; set; }
        public DateTime Date { get; set; }
        public List<UpdateExtremePhenomenonDetailDto> Details { get; set; }
    }
}
