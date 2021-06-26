using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Authentication;
using GloboWeather.WeatherManagement.Application.Models.Authentication.CreateUserRequest;
using GloboWeather.WeatherManagement.Application.Models.Authentication.Quiries.GetUsersList;
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
            return Ok(await _authenticationService.RegisterAsync(request: request));
        }

        [HttpPut("updateProfile")]
        public async Task<ActionResult<string>> UpdateProfile(UpdatingRequest request)
        {
            return Ok(await _authenticationService.UpdateUserProfileAsync(request: request));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<RoleResponse>> GetAllRoles()
        {
            return Ok(await _authenticationService.GetRolesListAsync());
        }

        [HttpPost("GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetUserListResponse>> GetAllUserList([FromBody] GetUsersListQuery query)
        {
            return Ok(await _authenticationService.GetUserListAsync(query));
        }
        
        
        [HttpPost("createUser")]
        public async Task<ActionResult<RegistrationResponse>> CreateUserAsync(CreateUserCommand request)
        {
            return Ok(await _authenticationService.CreateUserAsync(request: request));
        }

    }
}