
using GloboWeather.WeatherManegement.Application.Responses;

namespace GloboWeather.WeatherManagement.Application.Features.Commons.Commands.CreateStatus
{
    public class CreateStatusCommandResponse : BaseResponse
    {
        public CreateStatusCommandResponse() : base()
        {
            
        }
        public CreateStatusDto Status { get; set; }
    }
}