using System.Threading.Tasks;
using GloboWeather.WeatherManegement.Application.Models.Authentication;

namespace GloboWeather.WeatherManegement.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
    }
}