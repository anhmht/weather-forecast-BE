using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Monitoring.Mapping
{
    public class TramKttvMap : EntityTypeConfiguration<TramKttv>
    {
        public override void Map(EntityTypeBuilder<TramKttv> builder)
        {
            // Primary Key
            builder.HasKey(t => t.StationId);
            builder.ToTable("TramKttv");
        }
    }
}
