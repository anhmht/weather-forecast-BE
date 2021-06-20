using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboWeather.WeatherManagement.Monitoring.Mapping
{
    public class HydrologicalForecastMap : EntityTypeConfiguration<HydrologicalForecast>
    {
        public override void Map(EntityTypeBuilder<HydrologicalForecast> builder)
        {
            builder.HasKey(t => t.StationId);
            builder.ToTable("dbthuyvan");

            builder.Property(e => e.StationId)
                .HasColumnName("diemid");

            builder.Property(e => e.MinMax)
                .HasColumnName("minmax");
            builder.Property(e => e.RefDate)
                .HasColumnName("refdate");
            builder.Property(e => e.Day1)
                .HasColumnName("day1");
            builder.Property(e => e.Day2)
                .HasColumnName("day2");
            builder.Property(e => e.Day3)
                .HasColumnName("day3");
            builder.Property(e => e.Day4)
                .HasColumnName("day4");
            builder.Property(e => e.Day5)
                .HasColumnName("day5");
        }
    }
}
