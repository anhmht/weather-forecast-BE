using System;

namespace GloboWeather.WeatherManagement.Application.Models.Media
{
    public class MediaVideoSettings
    {
        public string AadClientId { get; set; }
        public string AadSecret { get; set; }
        public string AadTenantDomain { get; set; }
        public string AadTenantId { get; set; }
        public string AccountName { get; set; }
        public string ResourceGroup { get; set; }
        public string SubscriptionId { get; set; }
        public Uri ArmAadAudience { get; set; }
        public Uri ArmEndPoint { get; set; }
    }
}