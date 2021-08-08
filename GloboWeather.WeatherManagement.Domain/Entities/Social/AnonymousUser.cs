using System;
using System.ComponentModel.DataAnnotations;

namespace GloboWeather.WeatherManagement.Domain.Entities.Social
{
    public class AnonymousUser
    {
        [Key]
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
