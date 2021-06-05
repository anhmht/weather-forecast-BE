using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Application.Models.Monitoring
{
    public class TramKttvResponse
    {
        public string StationId { get; set; }
        public string Ten { get; set; }
        public float Y { get; set; }
        public float X { get; set; }
        public string LoaiTram { get; set; }
        public string CQQuanly { get; set; }

        public int MaTinh { get; set; }

        public string Diachi { get; set; }

        public string Hong { get; set; }

        public string CheDo { get; set; }

    }
}
