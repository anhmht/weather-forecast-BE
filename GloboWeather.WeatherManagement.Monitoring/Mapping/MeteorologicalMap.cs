using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboWeather.WeatherManagement.Monitoring.Mapping
{
    public class MeteorologicalMap : EntityTypeConfiguration<Meteorological>
    {
        public override void Map(EntityTypeBuilder<Meteorological> builder)
        {
            builder.HasKey(t => t.StationId);
            builder.ToTable("khituong");

            builder.Property(e => e.StationId)
                .HasColumnName("stationid");
            
            builder.Property(e => e.Date)
                .HasColumnName("dt");
            builder.Property(e => e.Evaporation)
                .HasColumnName("bochoi");
            builder.Property(e => e.Radiation)
                .HasColumnName("bucxa");
            builder.Property(e => e.Humidity)
                .HasColumnName("doam");
            builder.Property(e => e.Barometric)
                .HasColumnName("khiap");
            builder.Property(e => e.WindDirection)
                .HasColumnName("huonggio");
            builder.Property(e => e.Hga10)
                .HasColumnName("hga10");
            builder.Property(e => e.Hgm60)
                .HasColumnName("hgm60");
            builder.Property(e => e.Tdga10)
                .HasColumnName("tdga10");
            builder.Property(e => e.Tdgm60)
                .HasColumnName("tdgm60");
            builder.Property(e => e.WindSpeed)
                .HasColumnName("tocdogio");
            builder.Property(e => e.ZluyKe)
                .HasColumnName("zluyke");
            builder.Property(e => e.Rain)
                .HasColumnName("mua");
            builder.Property(e => e.Temperature)
                .HasColumnName("nhietdo");

        }
    }
}