using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherStates.Queries.GetWeatherStateDetail
{
    public class GetWeatherStateDetailQuery: IRequest<WeatherStateDetailVm>
    {
        public Guid Id { get; set; }
    }
}