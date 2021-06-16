using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboWeather.WeatherManagement.Monitoring.Mapping
{
    public class HydrologicalMap : EntityTypeConfiguration<Hydrological>
    {
        public override void Map(EntityTypeBuilder<Hydrological> builder)
        {
            builder.HasKey(t => t.StationId);
            builder.ToTable("thuyvan");

            builder.Property(e => e.StationId)
                .HasColumnName("stationid");
            
            builder.Property(e => e.Date)
                .HasColumnName("dt");
            builder.Property(e => e.Rain)
                .HasColumnName("mua");
            builder.Property(e => e.WaterLevel)
                .HasColumnName("mucnuoc");
            builder.Property(e => e.ZLuyKe)
                .HasColumnName("Zluyke");
        }
    }
}