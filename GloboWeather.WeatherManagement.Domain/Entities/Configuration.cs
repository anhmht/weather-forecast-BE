using System;
using System.ComponentModel.DataAnnotations;
using GloboWeather.WeatherManagement.Domain.Common;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class Configuration: AuditableEntity
    {
        public Guid ConfigurationId { get; set; }
        
        [MaxLength(50)] 
        public string AnalyticsCode { get; set; }
    
        [MaxLength(50)]
        public string EmailAddress { get; set; }
        
        [MaxLength(100)]
        public string EmailSenderName { get; set; }
        
        [MaxLength(255)]
        public string SendGridKey { get; set; }
        
        public string RegistrationApprovalMessage { get; set; }
    }
}