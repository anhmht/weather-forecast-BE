using System.Threading.Tasks;
using Microsoft.Azure.Management.Media;
using Microsoft.Identity.Client;
using Microsoft.Rest;

namespace GloboWeather.WeatherManagement.Infrastructure.Utils
{
    public class Authentication
    {
        public static readonly string TokenType = "Bearer";

        // public async Task<IAzureMediaServicesClient> CreateMediaServicesClientAsync(ConfigWrapper config,
        //     bool interactive = false)
        // {
        //     ServiceClientCredentials credentials;
        //     if (interactive)
        //     {
        //         credentials = await  G
        //     }
        // }

        // public async Task<ServiceClientCredentials> GetCredentialAsync(ConfigWrapper config)
        // {
        //     var scopes = new[] {config.ArmAadAudience + "/.default"};
        //     var app = ConfidentialClientApplicationBuilder.Create()
        // }
    }
}