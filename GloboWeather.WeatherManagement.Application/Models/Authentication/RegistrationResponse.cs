using System.Threading.Tasks.Dataflow;
using GloboWeather.WeatherManagement.Application.Responses;

namespace GloboWeather.WeatherManegement.Application.Models.Authentication
{
    public class RegistrationResponse : BaseResponse
    {
        public string UserId { get; set; }
    }
}