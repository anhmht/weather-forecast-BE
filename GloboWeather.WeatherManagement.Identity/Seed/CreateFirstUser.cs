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
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                IsActive = true
            };

            var user = await userManager.FindByEmailAsync(applicationUser.Email);
            if (user == null)
            {
                
                var createPowerUser =  await userManager.CreateAsync(applicationUser, "Admin!23");
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicationUser, "SUPERADMIN");
                }
            }
        }
    }
}