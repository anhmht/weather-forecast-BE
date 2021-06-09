using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboWeather.WeatherManagement.Monitoring.Mapping
{
    public class ProvinceMap:  EntityTypeConfiguration<Province>
    {
        public override void Map(EntityTypeBuilder<Province> builder)
        {
            builder.HasKey(t => t.ZipCode);
            builder.ToTable("tinh_hq");
            
            builder.Property(e => e.ZipCode)
                .HasColumnName("matinhVN");
            builder.Property(e => e.Code)
                .HasColumnName("maso");
            builder.Property(e => e.Name)
                .HasColumnName("ten");
        }
    }
}