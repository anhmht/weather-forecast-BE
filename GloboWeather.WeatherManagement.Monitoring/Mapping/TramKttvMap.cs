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

            builder.Property(e => e.StationId)
                .HasColumnName("stationid");
            builder.Property(e => e.Name)
                .HasColumnName("Ten");
            builder.Property(e => e.Lat)
                .HasColumnName("Y");
            builder.Property(e => e.Lon)
                .HasColumnName("X");
            builder.Property(e => e.StationType)
                .HasColumnName("LoaiTram");
            builder.Property(e => e.CQManagement)
                .HasColumnName("CQQuanLy");
            builder.Property(e => e.ZipCode)
                .HasColumnName("MaTinh");
            builder.Property(e => e.Address)
                .HasColumnName("DiaChi");
       
            
        }
    }
}
