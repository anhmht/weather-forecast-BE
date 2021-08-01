using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Authentication;
using GloboWeather.WeatherManagement.Application.Models.Authentication.ChangePassword;
using GloboWeather.WeatherManagement.Application.Models.Authentication.ConfirmEmail;
using GloboWeather.WeatherManagement.Application.Models.Authentication.CreateUserRequest;
using GloboWeather.WeatherManagement.Application.Models.Authentication.Quiries.GetUsersList;
using GloboWeather.WeatherManagement.Application.Models.Authentication.ResetPassword;
using GloboWeather.WeatherManegement.Application.Contracts.Identity;
using GloboWeather.WeatherManegement.Application.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _authenticationService.AuthenticateAsync(request));
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> RegisterAsync(RegistrationRequest request)
        {
            var response = await _authenticationService.RegisterAsync(request);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("updateProfile")]
        public async Task<ActionResult<string>> UpdateProfile(UpdatingRequest request)
        {
            return Ok(await _authenticationService.UpdateUserProfileAsync(request: request));
        }

        [HttpGet("GetAllRoles")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<RoleResponse>> GetAllRoles()
        {
            return Ok(await _authenticationService.GetRolesListAsync());
        }

        [HttpPost("GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<GetUserListResponse>> GetAllUserList([FromBody] GetUsersListQuery query)
        {
            return Ok(await _authenticationService.GetUserListAsync(query));
        }


        [HttpPost("createUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<RegistrationResponse>> CreateUserAsync(CreateUserCommand request)
        {
            var result = await _authenticationService.CreateUserAsync(request: request);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("get-user-info")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetUserInfo()
        {
            var email = HttpContext.User?.FindFirstValue(claimType: ClaimTypes.Email);
            return Ok(await _authenticationService.GetUserInfoAsync(email));
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var response = await _authenticationService.ForgotPasswordAsync(request.Email);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault().ErrorMessage));

            var response = await _authenticationService.ResetPasswordAsync(request);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("resend-verification-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ResendVerificationEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is not empty");
            }

            var response = await _authenticationService.ResendVerificationEmail(email);

            return Ok(response);
        }

        [HttpPost("confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var response = await _authenticationService.ConfirmEmailAsync(request);
            return Ok(response);
        }

        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var response = await _authenticationService.ChangePasswordAsync(request);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpDelete("{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteByEmailAsync([FromRoute] string email)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _authenticationService.DeleteUserByEmailAsync(email);
            
            return Ok();
        }

        [HttpGet("get-user-detail/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetUserInfoByIdAsync([FromRoute]string userId)
        {
            return Ok(await _authenticationService.GetUserInfoAsync(userId));
        }

    }
}