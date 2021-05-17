using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace GloboWeather.WeatherManagement.Identity.Seed
{
    public static class UserCreator
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var applicationUser = new ApplicationUser()
            {
                FirstName = "Phuong",
                LastName = "Last",
                UserName = "zigzac",
                Email = "phuongle@test.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(applicationUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(applicationUser, "Phu@ng123");
            }
        }
    }
}