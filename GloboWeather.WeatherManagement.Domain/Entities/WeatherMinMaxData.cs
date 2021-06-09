using GloboWeather.WeatherManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Domain.Entities
{
    public class WeatherMinMaxData : AuditableEntity
    {
        public Guid Id { get; set; }

        public int Type { get; set; }

        public DateTime RefDate { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }
    }
}
