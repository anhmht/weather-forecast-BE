using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboWeather.WeatherManagement.Monitoring.Mapping
{
    public class RainMap : EntityTypeConfiguration<Rain>
    {
        public override void Map(EntityTypeBuilder<Rain> builder)
        {
            builder.HasKey(t => t.StationId);
            builder.ToTable("mua");

            builder.Property(e => e.StationId)
                .HasColumnName("stationid");
            builder.Property(e => e.Date)
                .HasColumnName("dt");
            builder.Property(e => e.Quality)
                .HasColumnName("mua");
           
        }
    }
}