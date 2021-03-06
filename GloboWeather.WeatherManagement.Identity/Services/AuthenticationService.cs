using GloboWeather.WeatherManegement.Application.Contracts.Identity;
using GloboWeather.WeatherManegement.Application.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Caches;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Application.Models.Authentication;
using GloboWeather.WeatherManagement.Application.Models.Authentication.ChangePassword;
using GloboWeather.WeatherManagement.Application.Models.Authentication.ConfirmEmail;
using GloboWeather.WeatherManagement.Application.Models.Authentication.CreateUserRequest;
using GloboWeather.WeatherManagement.Application.Models.Authentication.Quiries.GetUsersList;
using GloboWeather.WeatherManagement.Application.Models.Authentication.ResetPassword;
using GloboWeather.WeatherManagement.Identity.Models;
using GloboWeather.WeatherManegement.Application.Contracts.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly ICacheStore _cacheStore;


        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            ICacheStore cacheStore
        )
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _cacheStore = cacheStore;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception($"User with {request.Email} not found.");
            }

            if (!user.EmailConfirmed)
            {
                throw new Exception($"User with {request.Email} didn't confirm");
            }

            if (user.IsDeleted == true)
            {
                throw new Exception($"User with email {request.Email} is deleted");
            }

            var result =
                await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false,
                    lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new Exception($"Credentials for {request.Email} aren't valid.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);
            AuthenticationResponse response = new AuthenticationResponse()
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
                Roles = userRoles.ToList()
            };
            return response;
        }

        public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
        {
            var registrationResponse = new RegistrationResponse();
            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                registrationResponse.Success = false;
                registrationResponse.Message = $"UserName {request.UserName} already exists.";

                return registrationResponse;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                CreatedOn = DateTime.Now,
                EmailConfirmed = false,
                IsActive = true
            };
            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                    var callbackUrl =
                        $"https://anhmht.github.io/weather-forecast-FE/#/confirm-email?uid={user.Id}&code={System.Net.WebUtility.UrlEncode(code)}";

                    await _emailService.SendEmailConfirmationAsync(request.Email, callbackUrl).ConfigureAwait(false);

                    await RefreshUserCacheAsync();

                    return new RegistrationResponse() {UserId = user.Id, Code = code};
                }
                else
                {
                    registrationResponse.Success = false;
                    registrationResponse.ValidationErrors.AddRange(result.Errors.Select(_ => _.Description));
                }
            }
            else
            {
                registrationResponse.Success = false;
                registrationResponse.Message = $"Email {request.Email} already exists.";
            }

            return registrationResponse;
        }

        public async Task<string> UpdateUserProfileAsync(UpdatingRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new Exception($"User with {request.UserName} not found");
            }

            //Map Model
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.AvatarUrl = request.AvatarUrl;
            user.PhoneNumber = request.PhoneNumber;

            #region Update roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var currentRolesString = string.Join(Constants.SemiColonStringSeparator, currentRoles.OrderBy(x => x));
            var newRolesString = string.Join(Constants.SemiColonStringSeparator, request.RoleNames.OrderBy(x => x));

            if(currentRolesString != newRolesString)
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRolesAsync(user, request.RoleNames);
            }
            #endregion

            var result = (await _userManager.UpdateAsync(user)).ToString();

            await RefreshUserCacheAsync();
            return result;
        }

        public async Task<List<RoleResponse>> GetRolesListAsync()
        {
            var roles = _roleManager.Roles.Select(x => new RoleResponse()
            {
                Name = x.Name,
                NormalizedName = x.NormalizedName,
                Id = x.Id
            }).ToList();

            return await Task.FromResult(roles);
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUserCommand request)
        {
            var createUserResponse = new CreateUserResponse();
            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                createUserResponse.Success = false;
                createUserResponse.Message = $"UserName {request.UserName} already exists.";
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                AvatarUrl = request.AvatarUrl,
                CreatedOn = DateTime.Now,
                EmailConfirmed = true,
                IsActive = true
            };
            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    if (request.RoleNames.Any())
                    {
                        await _userManager.AddToRolesAsync(user, request.RoleNames);
                    }

                    await RefreshUserCacheAsync();

                    return new CreateUserResponse() {UserId = user.Id};
                }
                else
                {
                    createUserResponse.Success = false;
                    createUserResponse.ValidationErrors = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        createUserResponse.ValidationErrors.Add(error.Description);
                    }
                }
            }
            else
            {
                createUserResponse.Success = false;
                createUserResponse.Message = $"Email {request.Email} already exists.";
            }

            return createUserResponse;
        }

        public async Task<GetUserListResponse> GetUserListAsync(GetUsersListQuery query)
        {
            PagedModel<ApplicationUser> userPaging;
            if (query.RoleIds == null || query.RoleIds.All(x => x.Equals(string.Empty)))
            {
                userPaging = await _userManager.Users.Where(x=>x.IsDeleted == null || x.IsDeleted == false).AsNoTracking()
                    .PaginateAsync(query.Page, query.Limit, new CancellationToken());
            }
            else
            {
                var userRoles = new List<ApplicationUser>();
                foreach (var roleName in query.RoleIds)
                {
                    userRoles.AddRange((await _userManager.GetUsersInRoleAsync(roleName)).Where(x=>x.IsDeleted == null || x.IsDeleted == false));
                }

                userPaging = userRoles.Distinct().Paginate(query.Page, query.Limit);
            }

            var usersResponse = new GetUserListResponse
            {
                CurrentPage = userPaging.CurrentPage,
                TotalItems = userPaging.TotalItems,
                TotalPages = userPaging.TotalPages,
                Users = new List<UserListVm>()
            };
            foreach (var user in userPaging.Items)
            {
                var userVm = new UserListVm
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    UserId = user.Id,
                    AvatarUrl = user.AvatarUrl,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedOn = user.CreatedOn,
                    RoleName = string.Join(",", (await _userManager.GetRolesAsync(user)).ToList()),
                    IsActive = user.IsActive
                };
                usersResponse.Users.Add(userVm);
            }

            return usersResponse;
        }

        public async Task<AuthenticationResponse> GetUserDetailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception($"User with {userId} not found.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            AuthenticationResponse response = new AuthenticationResponse()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
                Roles = userRoles.ToList()
            };
            return response;
        }

        public async Task<AuthenticationResponse> GetUserInfoAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception($"User with {email} not found.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            AuthenticationResponse response = new AuthenticationResponse()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
                Roles = userRoles.ToList()
            };
            return response;
        }
        
        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("uid", user.Id)
                }
                .Union(userClaims)
                .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signinCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signinCredential);

            return jwtSecurityToken;
        }

        public async Task<List<ApplicationUserDto>> GetAllUserAsync(bool isGetDeleted)
        {
            //Check to get from cache
            var applicationUserCacheKey = new ApplicationUserCacheKey();
            var cachedApplicationUser = _cacheStore.Get(applicationUserCacheKey);
            if (cachedApplicationUser != null)
                return cachedApplicationUser;

            //Get from database
            var users = await _userManager.Users.Where(x => x.IsDeleted == null || x.IsDeleted == false || isGetDeleted).ToListAsync();
            
            var response = GetApplicationUserDtos(users);

            //Save cache
            _cacheStore.Add(response, applicationUserCacheKey);

            return response;
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(string email)
        {
            var response = new ForgotPasswordResponse();
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false)))
            {
                response.Success = false;
                response.Message = "Please verify your email address";

                return response;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
            var callbackUrl =
                $"https://anhmht.github.io/weather-forecast-FE/#/reset-password?uid={user.Id}&code={System.Net.WebUtility.UrlEncode(code)}";

            await _emailService.SendEmailConfirmationAsync(email, callbackUrl).ConfigureAwait(false);
            response.Code = code;
            response.UserId = user.Id;

            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = new ResetPasswordResponse();
            var user = await _userManager.FindByIdAsync(request.UserId).ConfigureAwait(false);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalid credentials.";

                return response;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Code, request.Password)
                .ConfigureAwait(false);
            if (result.Errors.Any())
            {
                response.Success = false;

                response.ValidationErrors.AddRange(result.Errors.Select(x => x.Description));
            }

            return response;
        }

        public async Task<ConfirmEmailResponse> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            var response = new ConfirmEmailResponse();
            if (request.UserId == null || request.Code == null)
            {
                response.Success = false;
                response.Message = "Error retrieving information!";
            }

            var user = await _userManager.FindByIdAsync(request.UserId).ConfigureAwait(false);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Could not find User";
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Code).ConfigureAwait(false);
            if (result.Errors.Any())
            {
                response.Success = false;
                response.ValidationErrors.AddRange(result.Errors.Select(_ => _.Description));
            }

            if (response.Success)
            {
                await _userManager.AddToRoleAsync(user, "NORMALUSER");
            }

            return response;
        }

        public async Task<(string UserId, string Code)> ResendVerificationEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            //TODO: Check user is null
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
            var callbackUrl =
                $"https://anhmht.github.io/weather-forecast-FE/#/confirm-email?uid={user.Id}&code={System.Net.WebUtility.UrlEncode(code)}";

            await _emailService.SendEmailConfirmationAsync(email, callbackUrl).ConfigureAwait(false);
            return (UserId: user.Id, Code: code);
        }

        public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var response = new ChangePasswordResponse();
            var user = await _userManager.FindByIdAsync(userId: request.UserId).ConfigureAwait(false);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Please verify your email address";

                return response;
            }

            var result = await _userManager.ChangePasswordAsync(user,  request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                response.Success = false;
                response.ValidationErrors.AddRange(result.Errors.Select(_ => _.Description));
            }

            return response;
        }

        public async Task DeleteUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.Email = $"{user.Email}_deleted";
                user.UserName = $"{user.UserName}_deleted";
                user.IsDeleted = true;
                await _userManager.UpdateAsync(user);

                await RefreshUserCacheAsync();
            }
            else
            {
                throw new Exception($"User with email {email} not found.");
            }
        }

        public async Task<AuthenticationResponse> GetUserInfoByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new Exception($"User name {userName} not found.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var response = new AuthenticationResponse()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
                Roles = userRoles.ToList()
            };
            return response;
        }

        public async Task<List<ApplicationUserDto>> GetUsersInRoleAsync(string roleName)
        {
            var result= (await _userManager.GetUsersInRoleAsync(roleName)).ToList();

            return GetApplicationUserDtos(result);
        }

        private async Task RefreshUserCacheAsync()
        {
            var applicationUserCacheKey = new ApplicationUserCacheKey();

            var users = await _userManager.Users.Where(x => x.IsDeleted == null || x.IsDeleted == false).ToListAsync();

            var cacheUsers = GetApplicationUserDtos(users);

            //Save cache
            _cacheStore.Add(cacheUsers, applicationUserCacheKey);
        }

        private static List<ApplicationUserDto> GetApplicationUserDtos(List<ApplicationUser> users)
        {
            var cacheUsers = users.Select(x => new ApplicationUserDto()
            {
                FullName = $"{x.LastName} {x.FirstName}",
                UserName = x.UserName,
                AvatarUrl = x.AvatarUrl,
                ShortName = $"{x.LastName.First()}{x.FirstName.First()}"
            }).ToList();
            return cacheUsers;
        }
    }
}