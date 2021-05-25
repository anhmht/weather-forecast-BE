using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Commons.Commands.CreateStatus
{
    public class CreateStatusCommand : IRequest<CreateStatusCommandResponse>
    {
        public string Name { get; set; }
    }
}