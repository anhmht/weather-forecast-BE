
using GloboWeather.WeatherManegement.Application.Responses;

namespace GloboWeather.WeatherManagement.Application.Features.Commons.Commands.CreateStatus
{
    public class CreateStatusCommandResponse : BaseReponse
    {
        public CreateStatusCommandResponse() : base()
        {
            
        }
        public CreateStatusDto Status { get; set; }
    }
}