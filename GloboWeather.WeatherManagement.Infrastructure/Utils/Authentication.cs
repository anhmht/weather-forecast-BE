using System;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Media;
using Microsoft.Azure.Management.Media;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Rest;

using Microsoft.Extensions.Options;
using Serilog;

namespace GloboWeather.WeatherManagement.Infrastructure.Utils
{
    public class Authentication
    {
        public static readonly string TokenType = "Bearer";
        public MediaVideoSettings VideoSettings;
        public Authentication(IOptions<MediaVideoSettings> mediaConfig)
        {
            VideoSettings = mediaConfig.Value;
        }
        public static async Task<IAzureMediaServicesClient> CreateMediaServicesClientAsync(MediaVideoSettings VideoSettings,
            bool interactive = false)
        {
            ServiceClientCredentials credentials;
            if (interactive)
                credentials = await GetCredentialsInteractiveAuthAsync(VideoSettings);
            else
                credentials = await GetCredentialAsync(VideoSettings);

            return new AzureMediaServicesClient(VideoSettings.ArmEndPoint, credentials)
            {
                SubscriptionId = VideoSettings.SubscriptionId
            };
        }

    

        private static async Task<ServiceClientCredentials> GetCredentialAsync(MediaVideoSettings config)
        {
            var scopes = new[] {config.ArmAadAudience + "/.default"};
            var app = ConfidentialClientApplicationBuilder.Create(config.AadClientId)
                .WithClientSecret(config.AadSecret)
                .WithAuthority(AzureCloudInstance.AzurePublic, config.AadTenantId)
                .Build();

            var authResult = await app.AcquireTokenForClient(scopes)
                .ExecuteAsync()
                .ConfigureAwait(false);

            return new TokenCredentials(authResult.AccessToken, TokenType);
        }

        private static async Task<ServiceClientCredentials> GetCredentialsInteractiveAuthAsync(MediaVideoSettings config)
        {
            var scopes = new[] {config.ArmAadAudience + "/user_impersonation"};

            string clientApplicationId = "";
            
            AuthenticationResult result = null;

            IPublicClientApplication app = PublicClientApplicationBuilder.Create(clientApplicationId)
                .WithAuthority(AzureCloudInstance.AzurePublic, config.AadTenantId)
                .WithRedirectUri("http://localhost")
                .Build();

            var accounts = await app.GetAccountsAsync();

            try
            {
                result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();

            }
            catch (MsalUiRequiredException ex)
            {
                try
                {
                    result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
                }
                catch (MsalException msalException)
                {
                    Log.Error(msalException, $"ERROR: MSAL interactive authentication exception with code '{msalException.ErrorCode}' and message '{msalException.Message}'.");
                    Console.Error.WriteLine(
                        $"ERROR: MSAL interactive authentication exception with code '{msalException.ErrorCode}' and message '{msalException.Message}'.");
                }

            }
            catch ( MsalException msalException)
            {
                Log.Error(msalException, $"ERROR: MSAL silent authentication exception with code '{msalException.ErrorCode}' and message '{msalException.Message}'.");
                Console.Error.WriteLine($"ERROR: MSAL silent authentication exception with code '{msalException.ErrorCode}' and message '{msalException.Message}'.");
            }
            return new TokenCredentials(result.AccessToken, TokenType);
        }
    }
}