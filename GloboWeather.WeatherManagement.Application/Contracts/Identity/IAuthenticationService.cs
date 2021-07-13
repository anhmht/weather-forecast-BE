using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Events.Commands.CreateEvent;
using GloboWeather.WeatherManagement.Application.Models.Authentication;
using GloboWeather.WeatherManagement.Application.Models.Authentication.CreateUserRequest;
using GloboWeather.WeatherManagement.Application.Models.Authentication.Quiries.GetUsersList;
using GloboWeather.WeatherManegement.Application.Models.Authentication;

namespace GloboWeather.WeatherManegement.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);

        Task<string> UpdateUserProfileAsync(UpdatingRequest request);
        Task<List<RoleResponse>> GetRolesListAsync();
        Task<CreateUserResponse> CreateUserAsync(CreateUserCommand request);

        Task<GetUserListResponse> GetUserListAsync(GetUsersListQuery query);

        Task<AuthenticationResponse> GetUserInfoAsync(string email);

    //    Task<bool> DeleteUserAsync(string userId);

    }
}