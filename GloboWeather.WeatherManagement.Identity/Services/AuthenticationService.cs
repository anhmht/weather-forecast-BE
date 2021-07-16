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
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using GloboWeather.WeatherManagement.Application.Models.Authentication;
using GloboWeather.WeatherManagement.Application.Models.Authentication.CreateUserRequest;
using GloboWeather.WeatherManagement.Application.Models.Authentication.Quiries.GetUsersList;
using GloboWeather.WeatherManagement.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace GloboWeather.WeatherManagement.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManagement;
        private readonly JwtSettings _jwtSettings;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationService(
            UserManager<ApplicationUser> userManagement,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManagement = userManagement;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await _userManagement.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception($"User with {request.Email} not found.");
            }

            var result =
                await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false,
                    lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new Exception($"Credentials for {request.Email} aren't valid.");
            }

            var userRoles = await _userManagement.GetRolesAsync(user);

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
            var existingUser = await _userManagement.FindByNameAsync(request.UserName);
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
                EmailConfirmed = true,
                IsActive = true
            };
            var existingEmail = await _userManagement.FindByEmailAsync(request.Email);
            if (existingEmail == null)
            {
                var result = await _userManagement.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManagement.AddToRoleAsync(user, "NORMALUSER");
                    return new RegistrationResponse() {UserId = user.Id};
                }
                else
                {
                    registrationResponse.Success = false;
                    registrationResponse.ValidationErrors = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        registrationResponse.ValidationErrors.Add(error.Description);
                    }
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
            var user = await _userManagement.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new Exception($"User with {request.UserName} not found");
            }

            //Map Model
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.AvatarUrl = request.AvatarUrl;
            user.PhoneNumber = request.PhoneNumber;

            return (await _userManagement.UpdateAsync(user)).ToString();
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
            var existingUser = await _userManagement.FindByNameAsync(request.UserName);
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
                CreatedOn =  DateTime.Now,
                EmailConfirmed = true,
                IsActive = true
            };
            var existingEmail = await _userManagement.FindByEmailAsync(request.Email);
            if (existingEmail == null)
            {
                var result = await _userManagement.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    if (request.RoleNames.Any())
                    {
                        await _userManagement.AddToRolesAsync(user, request.RoleNames);
                    }

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
            
            // from userrole in UserRoles
            //     join user in Users on userrole.UserId equals user.Id
            //     where userrole.RoleId.Equals(role.Id)
            //     select user;
            PagedModel<ApplicationUser> userPaging;
            if (query.RoleIds == null || query.RoleIds.All(x => x.Equals(string.Empty)))
            {
                userPaging = await _userManagement.Users.AsNoTracking()
                    .PaginateAsync(query.Page, query.Limit, new CancellationToken());
            }
            else
            {
                var userRoles = new List<ApplicationUser>();
                foreach (var roleName in query.RoleIds)
                {
                    userRoles.AddRange(await _userManagement.GetUsersInRoleAsync(roleName));
                }

                userPaging = userRoles.Paginate(query.Page, query.Limit);
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
                    RoleName = string.Join(",", (await _userManagement.GetRolesAsync(user)).ToList()),
                    IsActive = user.IsActive
                };
                usersResponse.Users.Add(userVm);
            }
            return usersResponse;
        }

        public async Task<AuthenticationResponse> GetUserInfoAsync(string email)
        {
            var user = await _userManagement.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception($"User with {email} not found.");
            }
            
            var userRoles = await _userManagement.GetRolesAsync(user);
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

        // public async Task<bool> DeleteUserAsync(string userId)
        // {
        //     var user = await _userManagement.FindByIdAsync(userId);
        //     if (user == null)
        //     {
        //         throw new Exception($"User with {userId} not found.");
        //     }
        //
        //    var result=  await _userManagement.DeleteAsync(user);
        //    return  result.
        //
        // }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManagement.GetClaimsAsync(user);
            var roles = await _userManagement.GetRolesAsync(user);
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
    }
}