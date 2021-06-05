using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Monitoring.MonitoringEntities
{
    public class TramKttv
    {
        public string StationId { get; set; }
        public string Name { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public string StationType { get; set; }
        public string CQManagement { get; set; }

        public int ZipCode { get; set; }

        public string Address { get; set; }

        public string Hong { get; set; }
 

    }
}
