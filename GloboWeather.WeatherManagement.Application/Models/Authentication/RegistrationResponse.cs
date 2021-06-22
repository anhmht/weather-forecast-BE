using System.Threading.Tasks.Dataflow;
using GloboWeather.WeatherManegement.Application.Responses;

namespace GloboWeather.WeatherManegement.Application.Models.Authentication
{
    public class RegistrationResponse : BaseResponse
    {
        public string UserId { get; set; }
    }
}