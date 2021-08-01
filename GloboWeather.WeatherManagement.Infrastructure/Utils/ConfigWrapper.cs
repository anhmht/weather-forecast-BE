using System;
using Microsoft.Extensions.Configuration;

namespace GloboWeather.WeatherManagement.Infrastructure.Utils
{
    public class ConfigWrapper
    {
        private readonly IConfiguration _config;

        public ConfigWrapper(IConfiguration config)
        {
            _config = config;
        }

        public string SubscriptionId => _config["SubscriptionId"];
        public string ResourceGroup => _config["ResourceGroup"];
        public string AccountName => _config["AccountName"];
        public string AadTenantId => _config["AadTenantId"];
        public string AadClientId => _config["AadClientId"];
        public string AadSecret => _config["AadSecret"];
        public Uri ArmAadAudience => new(_config["ArmAadAudience"]);
        public Uri ArmEndPoint => new(_config["ArmEndpoint"]);

    }
}