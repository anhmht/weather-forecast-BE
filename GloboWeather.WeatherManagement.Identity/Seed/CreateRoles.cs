using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GloboWeather.WeatherManagement.Identity.Seed
{
    public static class RolesCreator
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = {"Admin", "SuperAdmin", "NormalUser", "DTH"};
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
       
    
    }
}