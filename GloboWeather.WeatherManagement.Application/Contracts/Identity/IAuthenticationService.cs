using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Authentication;
using GloboWeather.WeatherManegement.Application.Models.Authentication;

namespace GloboWeather.WeatherManegement.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);

        Task<string> UpdateUserProfileAsync(UpdatingRequest request);
        Task<List<RoleResponse>> GetRolesListAsync();
    }
}