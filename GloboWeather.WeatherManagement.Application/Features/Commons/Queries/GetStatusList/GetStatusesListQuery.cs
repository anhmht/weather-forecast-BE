using System.Collections.Generic;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Commons.Queries
{
    public class GetStatusesListQuery : IRequest<List<StatusesListVm>>
    {
        
    }
}