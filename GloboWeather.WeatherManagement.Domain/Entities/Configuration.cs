using System;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Configuration: AuditableEntity
    {
        public Guid ConfigurationId { get; set; }
        public string AnalyticsCode { get; set; }
        public string EmailAddress { get; set; }
        public string EmailSenderName { get; set; }
        public string SendGridKey { get; set; }
        public string RegistrationApprovalMessage { get; set; }
    }
}