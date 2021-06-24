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
                UserName = "zigzacvy",
                Email = "phuongle99@test.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(applicationUser.Email);
            if (user == null)
            {
                var createPowerUser =  await userManager.CreateAsync(applicationUser, "Phu@ng123");
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicationUser, "Admin");
                }
            }
        }
    }
}