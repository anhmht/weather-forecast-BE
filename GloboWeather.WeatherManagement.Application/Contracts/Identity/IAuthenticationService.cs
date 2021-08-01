using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Application.Models.Authentication;
using GloboWeather.WeatherManagement.Application.Models.Authentication.ChangePassword;
using GloboWeather.WeatherManagement.Application.Models.Authentication.ConfirmEmail;
using GloboWeather.WeatherManagement.Application.Models.Authentication.CreateUserRequest;
using GloboWeather.WeatherManagement.Application.Models.Authentication.Quiries.GetUsersList;
using GloboWeather.WeatherManagement.Application.Models.Authentication.ResetPassword;
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

        Task<AuthenticationResponse> GetUserDetailAsync(string userId);
        Task<AuthenticationResponse> GetUserInfoAsync(string email);

        Task<List<ApplicationUserDto>> GetAllUserAsync(bool isGetDeleted = false);

        Task<ForgotPasswordResponse> ForgotPasswordAsync(string email);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ConfirmEmailResponse> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<(string UserId, string Code)> ResendVerificationEmail(string email);
        Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request);
        Task DeleteUserByEmailAsync(string email);
    }
}