using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GloboWeather.WeatherManagement.Weather.weathercontext
{
    public partial class thoitietContext : DbContext
    {
        public thoitietContext()
        {
        }

        public thoitietContext(DbContextOptions<thoitietContext> options)
            : base(options)
        {
        }

        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=14.241.237.164; port=3306; database=thoitiet; user=moitruong; password=ttmt@123456; Persist Security Info=False; Connect Timeout=300");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Capgio>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("capgio");

                entity.Property(e => e.Color).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.WaveM).HasColumnName("Wave(m)");

                entity.Property(e => e.WindMS).HasColumnName("Wind(m/s)");
            });

            modelBuilder.Entity<Diemdubao>(entity =>
            {
                entity.ToTable("diemdubao");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(255);

                entity.Property(e => e.Ten).HasMaxLength(255);
            });

            modelBuilder.Entity<Htthoitiet>(entity =>
            {
                entity.ToTable("htthoitiet");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dem).HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.MoTa).HasMaxLength(255);

                entity.Property(e => e.Ngay).HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e._24h)
                    .HasColumnName("24h")
                    .HasColumnType("tinyint(3) unsigned");
            });

            modelBuilder.Entity<Icon24>(entity =>
            {
                entity.HasKey(e => e.Icon)
                    .HasName("PRIMARY");

                entity.ToTable("icon24");

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.Index).HasColumnType("int(11)");

                entity.Property(e => e.MoTa).HasMaxLength(255);
            });

            modelBuilder.Entity<Icondem>(entity =>
            {
                entity.HasKey(e => e.Icon)
                    .HasName("PRIMARY");

                entity.ToTable("icondem");

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.MoTa).HasMaxLength(255);
            });

            modelBuilder.Entity<Iconngay>(entity =>
            {
                entity.HasKey(e => e.Icon)
                    .HasName("PRIMARY");

                entity.ToTable("iconngay");

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.MoTa).HasMaxLength(255);
            });

            modelBuilder.Entity<Iconthoitiet>(entity =>
            {
                entity.ToTable("iconthoitiet");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.MoTa).HasMaxLength(255);
            });

            modelBuilder.Entity<May>(entity =>
            {
                entity.ToTable("may");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dem).HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.MoTa).HasMaxLength(255);

                entity.Property(e => e.Ngay).HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e._24h)
                    .HasColumnName("24h")
                    .HasColumnType("tinyint(3) unsigned");
            });

            modelBuilder.Entity<Mua>(entity =>
            {
                entity.ToTable("mua");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Dem).HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.MoTa).HasMaxLength(255);

                entity.Property(e => e.Ngay).HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e._24h)
                    .HasColumnName("24h")
                    .HasColumnType("tinyint(3) unsigned");
            });

            modelBuilder.Entity<Nhietdo>(entity =>
            {
                entity.HasKey(e => e.DiemId)
                    .HasName("PRIMARY");

                entity.ToTable("nhietdo");

                entity.HasIndex(e => e.DiemId)
                    .HasName("DiemId");

                entity.Property(e => e.DiemId).HasMaxLength(255);
                entity.Property(e => e.RefDate)
                    .HasColumnName("RefDate")
                    .HasColumnType("date");
                
                entity.Property(e => e._1)
                    .HasColumnName("1")
                    .HasColumnType("int(2)");

                entity.Property(e => e._10)
                    .HasColumnName("10")
                    .HasColumnType("int(2)");

                entity.Property(e => e._100)
                    .HasColumnName("100")
                    .HasColumnType("int(2)");

                entity.Property(e => e._101)
                    .HasColumnName("101")
                    .HasColumnType("int(2)");

                entity.Property(e => e._102)
                    .HasColumnName("102")
                    .HasColumnType("int(2)");

                entity.Property(e => e._103)
                    .HasColumnName("103")
                    .HasColumnType("int(2)");

                entity.Property(e => e._104)
                    .HasColumnName("104")
                    .HasColumnType("int(2)");

                entity.Property(e => e._105)
                    .HasColumnName("105")
                    .HasColumnType("int(2)");

                entity.Property(e => e._106)
                    .HasColumnName("106")
                    .HasColumnType("int(2)");

                entity.Property(e => e._107)
                    .HasColumnName("107")
                    .HasColumnType("int(2)");

                entity.Property(e => e._108)
                    .HasColumnName("108")
                    .HasColumnType("int(2)");

                entity.Property(e => e._109)
                    .HasColumnName("109")
                    .HasColumnType("int(2)");

                entity.Property(e => e._11)
                    .HasColumnName("11")
                    .HasColumnType("int(2)");

                entity.Property(e => e._110)
                    .HasColumnName("110")
                    .HasColumnType("int(2)");

                entity.Property(e => e._111)
                    .HasColumnName("111")
                    .HasColumnType("int(2)");

                entity.Property(e => e._112)
                    .HasColumnName("112")
                    .HasColumnType("int(2)");

                entity.Property(e => e._113)
                    .HasColumnName("113")
                    .HasColumnType("int(2)");

                entity.Property(e => e._114)
                    .HasColumnName("114")
                    .HasColumnType("int(2)");

                entity.Property(e => e._115)
                    .HasColumnName("115")
                    .HasColumnType("int(2)");

                entity.Property(e => e._116)
                    .HasColumnName("116")
                    .HasColumnType("int(2)");

                entity.Property(e => e._117)
                    .HasColumnName("117")
                    .HasColumnType("int(2)");

                entity.Property(e => e._118)
                    .HasColumnName("118")
                    .HasColumnType("int(2)");

                entity.Property(e => e._119)
                    .HasColumnName("119")
                    .HasColumnType("int(2)");

                entity.Property(e => e._12)
                    .HasColumnName("12")
                    .HasColumnType("int(2)");

                entity.Property(e => e._120)
                    .HasColumnName("120")
                    .HasColumnType("int(2)");

                entity.Property(e => e._13)
                    .HasColumnName("13")
                    .HasColumnType("int(2)");

                entity.Property(e => e._14)
                    .HasColumnName("14")
                    .HasColumnType("int(2)");

                entity.Property(e => e._15)
                    .HasColumnName("15")
                    .HasColumnType("int(2)");

                entity.Property(e => e._16)
                    .HasColumnName("16")
                    .HasColumnType("int(2)");

                entity.Property(e => e._17)
                    .HasColumnName("17")
                    .HasColumnType("int(2)");

                entity.Property(e => e._18)
                    .HasColumnName("18")
                    .HasColumnType("int(2)");

                entity.Property(e => e._19)
                    .HasColumnName("19")
                    .HasColumnType("int(2)");

                entity.Property(e => e._2)
                    .HasColumnName("2")
                    .HasColumnType("int(2)");

                entity.Property(e => e._20)
                    .HasColumnName("20")
                    .HasColumnType("int(2)");

                entity.Property(e => e._21)
                    .HasColumnName("21")
                    .HasColumnType("int(2)");

                entity.Property(e => e._22)
                    .HasColumnName("22")
                    .HasColumnType("int(2)");

                entity.Property(e => e._23)
                    .HasColumnName("23")
                    .HasColumnType("int(2)");

                entity.Property(e => e._24)
                    .HasColumnName("24")
                    .HasColumnType("int(2)");

                entity.Property(e => e._25)
                    .HasColumnName("25")
                    .HasColumnType("int(2)");

                entity.Property(e => e._26)
                    .HasColumnName("26")
                    .HasColumnType("int(2)");

                entity.Property(e => e._27)
                    .HasColumnName("27")
                    .HasColumnType("int(2)");

                entity.Property(e => e._28)
                    .HasColumnName("28")
                    .HasColumnType("int(2)");

                entity.Property(e => e._29)
                    .HasColumnName("29")
                    .HasColumnType("int(2)");

                entity.Property(e => e._3)
                    .HasColumnName("3")
                    .HasColumnType("int(2)");

                entity.Property(e => e._30)
                    .HasColumnName("30")
                    .HasColumnType("int(2)");

                entity.Property(e => e._31)
                    .HasColumnName("31")
                    .HasColumnType("int(2)");

                entity.Property(e => e._32)
                    .HasColumnName("32")
                    .HasColumnType("int(2)");

                entity.Property(e => e._33)
                    .HasColumnName("33")
                    .HasColumnType("int(2)");

                entity.Property(e => e._34)
                    .HasColumnName("34")
                    .HasColumnType("int(2)");

                entity.Property(e => e._35)
                    .HasColumnName("35")
                    .HasColumnType("int(2)");

                entity.Property(e => e._36)
                    .HasColumnName("36")
                    .HasColumnType("int(2)");

                entity.Property(e => e._37)
                    .HasColumnName("37")
                    .HasColumnType("int(2)");

                entity.Property(e => e._38)
                    .HasColumnName("38")
                    .HasColumnType("int(2)");

                entity.Property(e => e._39)
                    .HasColumnName("39")
                    .HasColumnType("int(2)");

                entity.Property(e => e._4)
                    .HasColumnName("4")
                    .HasColumnType("int(2)");

                entity.Property(e => e._40)
                    .HasColumnName("40")
                    .HasColumnType("int(2)");

                entity.Property(e => e._41)
                    .HasColumnName("41")
                    .HasColumnType("int(2)");

                entity.Property(e => e._42)
                    .HasColumnName("42")
                    .HasColumnType("int(2)");

                entity.Property(e => e._43)
                    .HasColumnName("43")
                    .HasColumnType("int(2)");

                entity.Property(e => e._44)
                    .HasColumnName("44")
                    .HasColumnType("int(2)");

                entity.Property(e => e._45)
                    .HasColumnName("45")
                    .HasColumnType("int(2)");

                entity.Property(e => e._46)
                    .HasColumnName("46")
                    .HasColumnType("int(2)");

                entity.Property(e => e._47)
                    .HasColumnName("47")
                    .HasColumnType("int(2)");

                entity.Property(e => e._48)
                    .HasColumnName("48")
                    .HasColumnType("int(2)");

                entity.Property(e => e._49)
                    .HasColumnName("49")
                    .HasColumnType("int(2)");

                entity.Property(e => e._5)
                    .HasColumnName("5")
                    .HasColumnType("int(2)");

                entity.Property(e => e._50)
                    .HasColumnName("50")
                    .HasColumnType("int(2)");

                entity.Property(e => e._51)
                    .HasColumnName("51")
                    .HasColumnType("int(2)");

                entity.Property(e => e._52)
                    .HasColumnName("52")
                    .HasColumnType("int(2)");

                entity.Property(e => e._53)
                    .HasColumnName("53")
                    .HasColumnType("int(2)");

                entity.Property(e => e._54)
                    .HasColumnName("54")
                    .HasColumnType("int(2)");

                entity.Property(e => e._55)
                    .HasColumnName("55")
                    .HasColumnType("int(2)");

                entity.Property(e => e._56)
                    .HasColumnName("56")
                    .HasColumnType("int(2)");

                entity.Property(e => e._57)
                    .HasColumnName("57")
                    .HasColumnType("int(2)");

                entity.Property(e => e._58)
                    .HasColumnName("58")
                    .HasColumnType("int(2)");

                entity.Property(e => e._59)
                    .HasColumnName("59")
                    .HasColumnType("int(2)");

                entity.Property(e => e._6)
                    .HasColumnName("6")
                    .HasColumnType("int(2)");

                entity.Property(e => e._60)
                    .HasColumnName("60")
                    .HasColumnType("int(2)");

                entity.Property(e => e._61)
                    .HasColumnName("61")
                    .HasColumnType("int(2)");

                entity.Property(e => e._62)
                    .HasColumnName("62")
                    .HasColumnType("int(2)");

                entity.Property(e => e._63)
                    .HasColumnName("63")
                    .HasColumnType("int(2)");

                entity.Property(e => e._64)
                    .HasColumnName("64")
                    .HasColumnType("int(2)");

                entity.Property(e => e._65)
                    .HasColumnName("65")
                    .HasColumnType("int(2)");

                entity.Property(e => e._66)
                    .HasColumnName("66")
                    .HasColumnType("int(2)");

                entity.Property(e => e._67)
                    .HasColumnName("67")
                    .HasColumnType("int(2)");

                entity.Property(e => e._68)
                    .HasColumnName("68")
                    .HasColumnType("int(2)");

                entity.Property(e => e._69)
                    .HasColumnName("69")
                    .HasColumnType("int(2)");

                entity.Property(e => e._7)
                    .HasColumnName("7")
                    .HasColumnType("int(2)");

                entity.Property(e => e._70)
                    .HasColumnName("70")
                    .HasColumnType("int(2)");

                entity.Property(e => e._71)
                    .HasColumnName("71")
                    .HasColumnType("int(2)");

                entity.Property(e => e._72)
                    .HasColumnName("72")
                    .HasColumnType("int(2)");

                entity.Property(e => e._73)
                    .HasColumnName("73")
                    .HasColumnType("int(2)");

                entity.Property(e => e._74)
                    .HasColumnName("74")
                    .HasColumnType("int(2)");

                entity.Property(e => e._75)
                    .HasColumnName("75")
                    .HasColumnType("int(2)");

                entity.Property(e => e._76)
                    .HasColumnName("76")
                    .HasColumnType("int(2)");

                entity.Property(e => e._77)
                    .HasColumnName("77")
                    .HasColumnType("int(2)");

                entity.Property(e => e._78)
                    .HasColumnName("78")
                    .HasColumnType("int(2)");

                entity.Property(e => e._79)
                    .HasColumnName("79")
                    .HasColumnType("int(2)");

                entity.Property(e => e._8)
                    .HasColumnName("8")
                    .HasColumnType("int(2)");

                entity.Property(e => e._80)
                    .HasColumnName("80")
                    .HasColumnType("int(2)");

                entity.Property(e => e._81)
                    .HasColumnName("81")
                    .HasColumnType("int(2)");

                entity.Property(e => e._82)
                    .HasColumnName("82")
                    .HasColumnType("int(2)");

                entity.Property(e => e._83)
                    .HasColumnName("83")
                    .HasColumnType("int(2)");

                entity.Property(e => e._84)
                    .HasColumnName("84")
                    .HasColumnType("int(2)");

                entity.Property(e => e._85)
                    .HasColumnName("85")
                    .HasColumnType("int(2)");

                entity.Property(e => e._86)
                    .HasColumnName("86")
                    .HasColumnType("int(2)");

                entity.Property(e => e._87)
                    .HasColumnName("87")
                    .HasColumnType("int(2)");

                entity.Property(e => e._88)
                    .HasColumnName("88")
                    .HasColumnType("int(2)");

                entity.Property(e => e._89)
                    .HasColumnName("89")
                    .HasColumnType("int(2)");

                entity.Property(e => e._9)
                    .HasColumnName("9")
                    .HasColumnType("int(2)");

                entity.Property(e => e._90)
                    .HasColumnName("90")
                    .HasColumnType("int(2)");

                entity.Property(e => e._91)
                    .HasColumnName("91")
                    .HasColumnType("int(2)");

                entity.Property(e => e._92)
                    .HasColumnName("92")
                    .HasColumnType("int(2)");

                entity.Property(e => e._93)
                    .HasColumnName("93")
                    .HasColumnType("int(2)");

                entity.Property(e => e._94)
                    .HasColumnName("94")
                    .HasColumnType("int(2)");

                entity.Property(e => e._95)
                    .HasColumnName("95")
                    .HasColumnType("int(2)");

                entity.Property(e => e._96)
                    .HasColumnName("96")
                    .HasColumnType("int(2)");

                entity.Property(e => e._97)
                    .HasColumnName("97")
                    .HasColumnType("int(2)");

                entity.Property(e => e._98)
                    .HasColumnName("98")
                    .HasColumnType("int(2)");

                entity.Property(e => e._99)
                    .HasColumnName("99")
                    .HasColumnType("int(2)");
            });

            modelBuilder.Entity<TocDoGio>(entity =>
            {
                entity.HasKey(e => e.DiemId)
                    .HasName("PRIMARY");

                entity.ToTable("tocdogio");

                entity.HasIndex(e => e.DiemId)
                    .HasName("DiemId");

                entity.Property(e => e.DiemId).HasMaxLength(255);
                entity.Property(e => e.RefDate)
                    .HasColumnName("RefDate")
                    .HasColumnType("date");

                entity.Property(e => e._1)
                    .HasColumnName("1")
                    .HasColumnType("int(2)");

                entity.Property(e => e._10)
                    .HasColumnName("10")
                    .HasColumnType("int(2)");

                entity.Property(e => e._100)
                    .HasColumnName("100")
                    .HasColumnType("int(2)");

                entity.Property(e => e._101)
                    .HasColumnName("101")
                    .HasColumnType("int(2)");

                entity.Property(e => e._102)
                    .HasColumnName("102")
                    .HasColumnType("int(2)");

                entity.Property(e => e._103)
                    .HasColumnName("103")
                    .HasColumnType("int(2)");

                entity.Property(e => e._104)
                    .HasColumnName("104")
                    .HasColumnType("int(2)");

                entity.Property(e => e._105)
                    .HasColumnName("105")
                    .HasColumnType("int(2)");

                entity.Property(e => e._106)
                    .HasColumnName("106")
                    .HasColumnType("int(2)");

                entity.Property(e => e._107)
                    .HasColumnName("107")
                    .HasColumnType("int(2)");

                entity.Property(e => e._108)
                    .HasColumnName("108")
                    .HasColumnType("int(2)");

                entity.Property(e => e._109)
                    .HasColumnName("109")
                    .HasColumnType("int(2)");

                entity.Property(e => e._11)
                    .HasColumnName("11")
                    .HasColumnType("int(2)");

                entity.Property(e => e._110)
                    .HasColumnName("110")
                    .HasColumnType("int(2)");

                entity.Property(e => e._111)
                    .HasColumnName("111")
                    .HasColumnType("int(2)");

                entity.Property(e => e._112)
                    .HasColumnName("112")
                    .HasColumnType("int(2)");

                entity.Property(e => e._113)
                    .HasColumnName("113")
                    .HasColumnType("int(2)");

                entity.Property(e => e._114)
                    .HasColumnName("114")
                    .HasColumnType("int(2)");

                entity.Property(e => e._115)
                    .HasColumnName("115")
                    .HasColumnType("int(2)");

                entity.Property(e => e._116)
                    .HasColumnName("116")
                    .HasColumnType("int(2)");

                entity.Property(e => e._117)
                    .HasColumnName("117")
                    .HasColumnType("int(2)");

                entity.Property(e => e._118)
                    .HasColumnName("118")
                    .HasColumnType("int(2)");

                entity.Property(e => e._119)
                    .HasColumnName("119")
                    .HasColumnType("int(2)");

                entity.Property(e => e._12)
                    .HasColumnName("12")
                    .HasColumnType("int(2)");

                entity.Property(e => e._120)
                    .HasColumnName("120")
                    .HasColumnType("int(2)");

                entity.Property(e => e._13)
                    .HasColumnName("13")
                    .HasColumnType("int(2)");

                entity.Property(e => e._14)
                    .HasColumnName("14")
                    .HasColumnType("int(2)");

                entity.Property(e => e._15)
                    .HasColumnName("15")
                    .HasColumnType("int(2)");

                entity.Property(e => e._16)
                    .HasColumnName("16")
                    .HasColumnType("int(2)");

                entity.Property(e => e._17)
                    .HasColumnName("17")
                    .HasColumnType("int(2)");

                entity.Property(e => e._18)
                    .HasColumnName("18")
                    .HasColumnType("int(2)");

                entity.Property(e => e._19)
                    .HasColumnName("19")
                    .HasColumnType("int(2)");

                entity.Property(e => e._2)
                    .HasColumnName("2")
                    .HasColumnType("int(2)");

                entity.Property(e => e._20)
                    .HasColumnName("20")
                    .HasColumnType("int(2)");

                entity.Property(e => e._21)
                    .HasColumnName("21")
                    .HasColumnType("int(2)");

                entity.Property(e => e._22)
                    .HasColumnName("22")
                    .HasColumnType("int(2)");

                entity.Property(e => e._23)
                    .HasColumnName("23")
                    .HasColumnType("int(2)");

                entity.Property(e => e._24)
                    .HasColumnName("24")
                    .HasColumnType("int(2)");

                entity.Property(e => e._25)
                    .HasColumnName("25")
                    .HasColumnType("int(2)");

                entity.Property(e => e._26)
                    .HasColumnName("26")
                    .HasColumnType("int(2)");

                entity.Property(e => e._27)
                    .HasColumnName("27")
                    .HasColumnType("int(2)");

                entity.Property(e => e._28)
                    .HasColumnName("28")
                    .HasColumnType("int(2)");

                entity.Property(e => e._29)
                    .HasColumnName("29")
                    .HasColumnType("int(2)");

                entity.Property(e => e._3)
                    .HasColumnName("3")
                    .HasColumnType("int(2)");

                entity.Property(e => e._30)
                    .HasColumnName("30")
                    .HasColumnType("int(2)");

                entity.Property(e => e._31)
                    .HasColumnName("31")
                    .HasColumnType("int(2)");

                entity.Property(e => e._32)
                    .HasColumnName("32")
                    .HasColumnType("int(2)");

                entity.Property(e => e._33)
                    .HasColumnName("33")
                    .HasColumnType("int(2)");

                entity.Property(e => e._34)
                    .HasColumnName("34")
                    .HasColumnType("int(2)");

                entity.Property(e => e._35)
                    .HasColumnName("35")
                    .HasColumnType("int(2)");

                entity.Property(e => e._36)
                    .HasColumnName("36")
                    .HasColumnType("int(2)");

                entity.Property(e => e._37)
                    .HasColumnName("37")
                    .HasColumnType("int(2)");

                entity.Property(e => e._38)
                    .HasColumnName("38")
                    .HasColumnType("int(2)");

                entity.Property(e => e._39)
                    .HasColumnName("39")
                    .HasColumnType("int(2)");

                entity.Property(e => e._4)
                    .HasColumnName("4")
                    .HasColumnType("int(2)");

                entity.Property(e => e._40)
                    .HasColumnName("40")
                    .HasColumnType("int(2)");

                entity.Property(e => e._41)
                    .HasColumnName("41")
                    .HasColumnType("int(2)");

                entity.Property(e => e._42)
                    .HasColumnName("42")
                    .HasColumnType("int(2)");

                entity.Property(e => e._43)
                    .HasColumnName("43")
                    .HasColumnType("int(2)");

                entity.Property(e => e._44)
                    .HasColumnName("44")
                    .HasColumnType("int(2)");

                entity.Property(e => e._45)
                    .HasColumnName("45")
                    .HasColumnType("int(2)");

                entity.Property(e => e._46)
                    .HasColumnName("46")
                    .HasColumnType("int(2)");

                entity.Property(e => e._47)
                    .HasColumnName("47")
                    .HasColumnType("int(2)");

                entity.Property(e => e._48)
                    .HasColumnName("48")
                    .HasColumnType("int(2)");

                entity.Property(e => e._49)
                    .HasColumnName("49")
                    .HasColumnType("int(2)");

                entity.Property(e => e._5)
                    .HasColumnName("5")
                    .HasColumnType("int(2)");

                entity.Property(e => e._50)
                    .HasColumnName("50")
                    .HasColumnType("int(2)");

                entity.Property(e => e._51)
                    .HasColumnName("51")
                    .HasColumnType("int(2)");

                entity.Property(e => e._52)
                    .HasColumnName("52")
                    .HasColumnType("int(2)");

                entity.Property(e => e._53)
                    .HasColumnName("53")
                    .HasColumnType("int(2)");

                entity.Property(e => e._54)
                    .HasColumnName("54")
                    .HasColumnType("int(2)");

                entity.Property(e => e._55)
                    .HasColumnName("55")
                    .HasColumnType("int(2)");

                entity.Property(e => e._56)
                    .HasColumnName("56")
                    .HasColumnType("int(2)");

                entity.Property(e => e._57)
                    .HasColumnName("57")
                    .HasColumnType("int(2)");

                entity.Property(e => e._58)
                    .HasColumnName("58")
                    .HasColumnType("int(2)");

                entity.Property(e => e._59)
                    .HasColumnName("59")
                    .HasColumnType("int(2)");

                entity.Property(e => e._6)
                    .HasColumnName("6")
                    .HasColumnType("int(2)");

                entity.Property(e => e._60)
                    .HasColumnName("60")
                    .HasColumnType("int(2)");

                entity.Property(e => e._61)
                    .HasColumnName("61")
                    .HasColumnType("int(2)");

                entity.Property(e => e._62)
                    .HasColumnName("62")
                    .HasColumnType("int(2)");

                entity.Property(e => e._63)
                    .HasColumnName("63")
                    .HasColumnType("int(2)");

                entity.Property(e => e._64)
                    .HasColumnName("64")
                    .HasColumnType("int(2)");

                entity.Property(e => e._65)
                    .HasColumnName("65")
                    .HasColumnType("int(2)");

                entity.Property(e => e._66)
                    .HasColumnName("66")
                    .HasColumnType("int(2)");

                entity.Property(e => e._67)
                    .HasColumnName("67")
                    .HasColumnType("int(2)");

                entity.Property(e => e._68)
                    .HasColumnName("68")
                    .HasColumnType("int(2)");

                entity.Property(e => e._69)
                    .HasColumnName("69")
                    .HasColumnType("int(2)");

                entity.Property(e => e._7)
                    .HasColumnName("7")
                    .HasColumnType("int(2)");

                entity.Property(e => e._70)
                    .HasColumnName("70")
                    .HasColumnType("int(2)");

                entity.Property(e => e._71)
                    .HasColumnName("71")
                    .HasColumnType("int(2)");

                entity.Property(e => e._72)
                    .HasColumnName("72")
                    .HasColumnType("int(2)");

                entity.Property(e => e._73)
                    .HasColumnName("73")
                    .HasColumnType("int(2)");

                entity.Property(e => e._74)
                    .HasColumnName("74")
                    .HasColumnType("int(2)");

                entity.Property(e => e._75)
                    .HasColumnName("75")
                    .HasColumnType("int(2)");

                entity.Property(e => e._76)
                    .HasColumnName("76")
                    .HasColumnType("int(2)");

                entity.Property(e => e._77)
                    .HasColumnName("77")
                    .HasColumnType("int(2)");

                entity.Property(e => e._78)
                    .HasColumnName("78")
                    .HasColumnType("int(2)");

                entity.Property(e => e._79)
                    .HasColumnName("79")
                    .HasColumnType("int(2)");

                entity.Property(e => e._8)
                    .HasColumnName("8")
                    .HasColumnType("int(2)");

                entity.Property(e => e._80)
                    .HasColumnName("80")
                    .HasColumnType("int(2)");

                entity.Property(e => e._81)
                    .HasColumnName("81")
                    .HasColumnType("int(2)");

                entity.Property(e => e._82)
                    .HasColumnName("82")
                    .HasColumnType("int(2)");

                entity.Property(e => e._83)
                    .HasColumnName("83")
                    .HasColumnType("int(2)");

                entity.Property(e => e._84)
                    .HasColumnName("84")
                    .HasColumnType("int(2)");

                entity.Property(e => e._85)
                    .HasColumnName("85")
                    .HasColumnType("int(2)");

                entity.Property(e => e._86)
                    .HasColumnName("86")
                    .HasColumnType("int(2)");

                entity.Property(e => e._87)
                    .HasColumnName("87")
                    .HasColumnType("int(2)");

                entity.Property(e => e._88)
                    .HasColumnName("88")
                    .HasColumnType("int(2)");

                entity.Property(e => e._89)
                    .HasColumnName("89")
                    .HasColumnType("int(2)");

                entity.Property(e => e._9)
                    .HasColumnName("9")
                    .HasColumnType("int(2)");

                entity.Property(e => e._90)
                    .HasColumnName("90")
                    .HasColumnType("int(2)");

                entity.Property(e => e._91)
                    .HasColumnName("91")
                    .HasColumnType("int(2)");

                entity.Property(e => e._92)
                    .HasColumnName("92")
                    .HasColumnType("int(2)");

                entity.Property(e => e._93)
                    .HasColumnName("93")
                    .HasColumnType("int(2)");

                entity.Property(e => e._94)
                    .HasColumnName("94")
                    .HasColumnType("int(2)");

                entity.Property(e => e._95)
                    .HasColumnName("95")
                    .HasColumnType("int(2)");

                entity.Property(e => e._96)
                    .HasColumnName("96")
                    .HasColumnType("int(2)");

                entity.Property(e => e._97)
                    .HasColumnName("97")
                    .HasColumnType("int(2)");

                entity.Property(e => e._98)
                    .HasColumnName("98")
                    .HasColumnType("int(2)");

                entity.Property(e => e._99)
                    .HasColumnName("99")
                    .HasColumnType("int(2)");
            });

            modelBuilder.Entity<DoAmTB>(entity =>
            {
                entity.HasKey(e => e.DiemId)
                    .HasName("PRIMARY");

                entity.ToTable("doamtb");

                entity.HasIndex(e => e.DiemId)
                    .HasName("DiemId");

                entity.Property(e => e.DiemId).HasMaxLength(255);
                entity.Property(e => e.RefDate)
                    .HasColumnName("RefDate")
                    .HasColumnType("date");

                entity.Property(e => e._1)
                    .HasColumnName("1")
                    .HasColumnType("int(2)");

                entity.Property(e => e._10)
                    .HasColumnName("10")
                    .HasColumnType("int(2)");

                entity.Property(e => e._100)
                    .HasColumnName("100")
                    .HasColumnType("int(2)");

                entity.Property(e => e._101)
                    .HasColumnName("101")
                    .HasColumnType("int(2)");

                entity.Property(e => e._102)
                    .HasColumnName("102")
                    .HasColumnType("int(2)");

                entity.Property(e => e._103)
                    .HasColumnName("103")
                    .HasColumnType("int(2)");

                entity.Property(e => e._104)
                    .HasColumnName("104")
                    .HasColumnType("int(2)");

                entity.Property(e => e._105)
                    .HasColumnName("105")
                    .HasColumnType("int(2)");

                entity.Property(e => e._106)
                    .HasColumnName("106")
                    .HasColumnType("int(2)");

                entity.Property(e => e._107)
                    .HasColumnName("107")
                    .HasColumnType("int(2)");

                entity.Property(e => e._108)
                    .HasColumnName("108")
                    .HasColumnType("int(2)");

                entity.Property(e => e._109)
                    .HasColumnName("109")
                    .HasColumnType("int(2)");

                entity.Property(e => e._11)
                    .HasColumnName("11")
                    .HasColumnType("int(2)");

                entity.Property(e => e._110)
                    .HasColumnName("110")
                    .HasColumnType("int(2)");

                entity.Property(e => e._111)
                    .HasColumnName("111")
                    .HasColumnType("int(2)");

                entity.Property(e => e._112)
                    .HasColumnName("112")
                    .HasColumnType("int(2)");

                entity.Property(e => e._113)
                    .HasColumnName("113")
                    .HasColumnType("int(2)");

                entity.Property(e => e._114)
                    .HasColumnName("114")
                    .HasColumnType("int(2)");

                entity.Property(e => e._115)
                    .HasColumnName("115")
                    .HasColumnType("int(2)");

                entity.Property(e => e._116)
                    .HasColumnName("116")
                    .HasColumnType("int(2)");

                entity.Property(e => e._117)
                    .HasColumnName("117")
                    .HasColumnType("int(2)");

                entity.Property(e => e._118)
                    .HasColumnName("118")
                    .HasColumnType("int(2)");

                entity.Property(e => e._119)
                    .HasColumnName("119")
                    .HasColumnType("int(2)");

                entity.Property(e => e._12)
                    .HasColumnName("12")
                    .HasColumnType("int(2)");

                entity.Property(e => e._120)
                    .HasColumnName("120")
                    .HasColumnType("int(2)");

                entity.Property(e => e._13)
                    .HasColumnName("13")
                    .HasColumnType("int(2)");

                entity.Property(e => e._14)
                    .HasColumnName("14")
                    .HasColumnType("int(2)");

                entity.Property(e => e._15)
                    .HasColumnName("15")
                    .HasColumnType("int(2)");

                entity.Property(e => e._16)
                    .HasColumnName("16")
                    .HasColumnType("int(2)");

                entity.Property(e => e._17)
                    .HasColumnName("17")
                    .HasColumnType("int(2)");

                entity.Property(e => e._18)
                    .HasColumnName("18")
                    .HasColumnType("int(2)");

                entity.Property(e => e._19)
                    .HasColumnName("19")
                    .HasColumnType("int(2)");

                entity.Property(e => e._2)
                    .HasColumnName("2")
                    .HasColumnType("int(2)");

                entity.Property(e => e._20)
                    .HasColumnName("20")
                    .HasColumnType("int(2)");

                entity.Property(e => e._21)
                    .HasColumnName("21")
                    .HasColumnType("int(2)");

                entity.Property(e => e._22)
                    .HasColumnName("22")
                    .HasColumnType("int(2)");

                entity.Property(e => e._23)
                    .HasColumnName("23")
                    .HasColumnType("int(2)");

                entity.Property(e => e._24)
                    .HasColumnName("24")
                    .HasColumnType("int(2)");

                entity.Property(e => e._25)
                    .HasColumnName("25")
                    .HasColumnType("int(2)");

                entity.Property(e => e._26)
                    .HasColumnName("26")
                    .HasColumnType("int(2)");

                entity.Property(e => e._27)
                    .HasColumnName("27")
                    .HasColumnType("int(2)");

                entity.Property(e => e._28)
                    .HasColumnName("28")
                    .HasColumnType("int(2)");

                entity.Property(e => e._29)
                    .HasColumnName("29")
                    .HasColumnType("int(2)");

                entity.Property(e => e._3)
                    .HasColumnName("3")
                    .HasColumnType("int(2)");

                entity.Property(e => e._30)
                    .HasColumnName("30")
                    .HasColumnType("int(2)");

                entity.Property(e => e._31)
                    .HasColumnName("31")
                    .HasColumnType("int(2)");

                entity.Property(e => e._32)
                    .HasColumnName("32")
                    .HasColumnType("int(2)");

                entity.Property(e => e._33)
                    .HasColumnName("33")
                    .HasColumnType("int(2)");

                entity.Property(e => e._34)
                    .HasColumnName("34")
                    .HasColumnType("int(2)");

                entity.Property(e => e._35)
                    .HasColumnName("35")
                    .HasColumnType("int(2)");

                entity.Property(e => e._36)
                    .HasColumnName("36")
                    .HasColumnType("int(2)");

                entity.Property(e => e._37)
                    .HasColumnName("37")
                    .HasColumnType("int(2)");

                entity.Property(e => e._38)
                    .HasColumnName("38")
                    .HasColumnType("int(2)");

                entity.Property(e => e._39)
                    .HasColumnName("39")
                    .HasColumnType("int(2)");

                entity.Property(e => e._4)
                    .HasColumnName("4")
                    .HasColumnType("int(2)");

                entity.Property(e => e._40)
                    .HasColumnName("40")
                    .HasColumnType("int(2)");

                entity.Property(e => e._41)
                    .HasColumnName("41")
                    .HasColumnType("int(2)");

                entity.Property(e => e._42)
                    .HasColumnName("42")
                    .HasColumnType("int(2)");

                entity.Property(e => e._43)
                    .HasColumnName("43")
                    .HasColumnType("int(2)");

                entity.Property(e => e._44)
                    .HasColumnName("44")
                    .HasColumnType("int(2)");

                entity.Property(e => e._45)
                    .HasColumnName("45")
                    .HasColumnType("int(2)");

                entity.Property(e => e._46)
                    .HasColumnName("46")
                    .HasColumnType("int(2)");

                entity.Property(e => e._47)
                    .HasColumnName("47")
                    .HasColumnType("int(2)");

                entity.Property(e => e._48)
                    .HasColumnName("48")
                    .HasColumnType("int(2)");

                entity.Property(e => e._49)
                    .HasColumnName("49")
                    .HasColumnType("int(2)");

                entity.Property(e => e._5)
                    .HasColumnName("5")
                    .HasColumnType("int(2)");

                entity.Property(e => e._50)
                    .HasColumnName("50")
                    .HasColumnType("int(2)");

                entity.Property(e => e._51)
                    .HasColumnName("51")
                    .HasColumnType("int(2)");

                entity.Property(e => e._52)
                    .HasColumnName("52")
                    .HasColumnType("int(2)");

                entity.Property(e => e._53)
                    .HasColumnName("53")
                    .HasColumnType("int(2)");

                entity.Property(e => e._54)
                    .HasColumnName("54")
                    .HasColumnType("int(2)");

                entity.Property(e => e._55)
                    .HasColumnName("55")
                    .HasColumnType("int(2)");

                entity.Property(e => e._56)
                    .HasColumnName("56")
                    .HasColumnType("int(2)");

                entity.Property(e => e._57)
                    .HasColumnName("57")
                    .HasColumnType("int(2)");

                entity.Property(e => e._58)
                    .HasColumnName("58")
                    .HasColumnType("int(2)");

                entity.Property(e => e._59)
                    .HasColumnName("59")
                    .HasColumnType("int(2)");

                entity.Property(e => e._6)
                    .HasColumnName("6")
                    .HasColumnType("int(2)");

                entity.Property(e => e._60)
                    .HasColumnName("60")
                    .HasColumnType("int(2)");

                entity.Property(e => e._61)
                    .HasColumnName("61")
                    .HasColumnType("int(2)");

                entity.Property(e => e._62)
                    .HasColumnName("62")
                    .HasColumnType("int(2)");

                entity.Property(e => e._63)
                    .HasColumnName("63")
                    .HasColumnType("int(2)");

                entity.Property(e => e._64)
                    .HasColumnName("64")
                    .HasColumnType("int(2)");

                entity.Property(e => e._65)
                    .HasColumnName("65")
                    .HasColumnType("int(2)");

                entity.Property(e => e._66)
                    .HasColumnName("66")
                    .HasColumnType("int(2)");

                entity.Property(e => e._67)
                    .HasColumnName("67")
                    .HasColumnType("int(2)");

                entity.Property(e => e._68)
                    .HasColumnName("68")
                    .HasColumnType("int(2)");

                entity.Property(e => e._69)
                    .HasColumnName("69")
                    .HasColumnType("int(2)");

                entity.Property(e => e._7)
                    .HasColumnName("7")
                    .HasColumnType("int(2)");

                entity.Property(e => e._70)
                    .HasColumnName("70")
                    .HasColumnType("int(2)");

                entity.Property(e => e._71)
                    .HasColumnName("71")
                    .HasColumnType("int(2)");

                entity.Property(e => e._72)
                    .HasColumnName("72")
                    .HasColumnType("int(2)");

                entity.Property(e => e._73)
                    .HasColumnName("73")
                    .HasColumnType("int(2)");

                entity.Property(e => e._74)
                    .HasColumnName("74")
                    .HasColumnType("int(2)");

                entity.Property(e => e._75)
                    .HasColumnName("75")
                    .HasColumnType("int(2)");

                entity.Property(e => e._76)
                    .HasColumnName("76")
                    .HasColumnType("int(2)");

                entity.Property(e => e._77)
                    .HasColumnName("77")
                    .HasColumnType("int(2)");

                entity.Property(e => e._78)
                    .HasColumnName("78")
                    .HasColumnType("int(2)");

                entity.Property(e => e._79)
                    .HasColumnName("79")
                    .HasColumnType("int(2)");

                entity.Property(e => e._8)
                    .HasColumnName("8")
                    .HasColumnType("int(2)");

                entity.Property(e => e._80)
                    .HasColumnName("80")
                    .HasColumnType("int(2)");

                entity.Property(e => e._81)
                    .HasColumnName("81")
                    .HasColumnType("int(2)");

                entity.Property(e => e._82)
                    .HasColumnName("82")
                    .HasColumnType("int(2)");

                entity.Property(e => e._83)
                    .HasColumnName("83")
                    .HasColumnType("int(2)");

                entity.Property(e => e._84)
                    .HasColumnName("84")
                    .HasColumnType("int(2)");

                entity.Property(e => e._85)
                    .HasColumnName("85")
                    .HasColumnType("int(2)");

                entity.Property(e => e._86)
                    .HasColumnName("86")
                    .HasColumnType("int(2)");

                entity.Property(e => e._87)
                    .HasColumnName("87")
                    .HasColumnType("int(2)");

                entity.Property(e => e._88)
                    .HasColumnName("88")
                    .HasColumnType("int(2)");

                entity.Property(e => e._89)
                    .HasColumnName("89")
                    .HasColumnType("int(2)");

                entity.Property(e => e._9)
                    .HasColumnName("9")
                    .HasColumnType("int(2)");

                entity.Property(e => e._90)
                    .HasColumnName("90")
                    .HasColumnType("int(2)");

                entity.Property(e => e._91)
                    .HasColumnName("91")
                    .HasColumnType("int(2)");

                entity.Property(e => e._92)
                    .HasColumnName("92")
                    .HasColumnType("int(2)");

                entity.Property(e => e._93)
                    .HasColumnName("93")
                    .HasColumnType("int(2)");

                entity.Property(e => e._94)
                    .HasColumnName("94")
                    .HasColumnType("int(2)");

                entity.Property(e => e._95)
                    .HasColumnName("95")
                    .HasColumnType("int(2)");

                entity.Property(e => e._96)
                    .HasColumnName("96")
                    .HasColumnType("int(2)");

                entity.Property(e => e._97)
                    .HasColumnName("97")
                    .HasColumnType("int(2)");

                entity.Property(e => e._98)
                    .HasColumnName("98")
                    .HasColumnType("int(2)");

                entity.Property(e => e._99)
                    .HasColumnName("99")
                    .HasColumnType("int(2)");
            });

            modelBuilder.Entity<AmountOfRain>(entity =>
            {
                entity.HasKey(e => e.DiemId)
                    .HasName("PRIMARY");

                entity.ToTable("luongmua");

                entity.HasIndex(e => e.DiemId)
                    .HasName("DiemId");

                entity.Property(e => e.DiemId).HasMaxLength(255);
                entity.Property(e => e.RefDate)
                    .HasColumnName("RefDate")
                    .HasColumnType("date");

                entity.Property(e => e._1)
                    .HasColumnName("1")
                    .HasColumnType("int(2)");

                entity.Property(e => e._10)
                    .HasColumnName("10")
                    .HasColumnType("int(2)");

                entity.Property(e => e._100)
                    .HasColumnName("100")
                    .HasColumnType("int(2)");

                entity.Property(e => e._101)
                    .HasColumnName("101")
                    .HasColumnType("int(2)");

                entity.Property(e => e._102)
                    .HasColumnName("102")
                    .HasColumnType("int(2)");

                entity.Property(e => e._103)
                    .HasColumnName("103")
                    .HasColumnType("int(2)");

                entity.Property(e => e._104)
                    .HasColumnName("104")
                    .HasColumnType("int(2)");

                entity.Property(e => e._105)
                    .HasColumnName("105")
                    .HasColumnType("int(2)");

                entity.Property(e => e._106)
                    .HasColumnName("106")
                    .HasColumnType("int(2)");

                entity.Property(e => e._107)
                    .HasColumnName("107")
                    .HasColumnType("int(2)");

                entity.Property(e => e._108)
                    .HasColumnName("108")
                    .HasColumnType("int(2)");

                entity.Property(e => e._109)
                    .HasColumnName("109")
                    .HasColumnType("int(2)");

                entity.Property(e => e._11)
                    .HasColumnName("11")
                    .HasColumnType("int(2)");

                entity.Property(e => e._110)
                    .HasColumnName("110")
                    .HasColumnType("int(2)");

                entity.Property(e => e._111)
                    .HasColumnName("111")
                    .HasColumnType("int(2)");

                entity.Property(e => e._112)
                    .HasColumnName("112")
                    .HasColumnType("int(2)");

                entity.Property(e => e._113)
                    .HasColumnName("113")
                    .HasColumnType("int(2)");

                entity.Property(e => e._114)
                    .HasColumnName("114")
                    .HasColumnType("int(2)");

                entity.Property(e => e._115)
                    .HasColumnName("115")
                    .HasColumnType("int(2)");

                entity.Property(e => e._116)
                    .HasColumnName("116")
                    .HasColumnType("int(2)");

                entity.Property(e => e._117)
                    .HasColumnName("117")
                    .HasColumnType("int(2)");

                entity.Property(e => e._118)
                    .HasColumnName("118")
                    .HasColumnType("int(2)");

                entity.Property(e => e._119)
                    .HasColumnName("119")
                    .HasColumnType("int(2)");

                entity.Property(e => e._12)
                    .HasColumnName("12")
                    .HasColumnType("int(2)");

                entity.Property(e => e._120)
                    .HasColumnName("120")
                    .HasColumnType("int(2)");

                entity.Property(e => e._13)
                    .HasColumnName("13")
                    .HasColumnType("int(2)");

                entity.Property(e => e._14)
                    .HasColumnName("14")
                    .HasColumnType("int(2)");

                entity.Property(e => e._15)
                    .HasColumnName("15")
                    .HasColumnType("int(2)");

                entity.Property(e => e._16)
                    .HasColumnName("16")
                    .HasColumnType("int(2)");

                entity.Property(e => e._17)
                    .HasColumnName("17")
                    .HasColumnType("int(2)");

                entity.Property(e => e._18)
                    .HasColumnName("18")
                    .HasColumnType("int(2)");

                entity.Property(e => e._19)
                    .HasColumnName("19")
                    .HasColumnType("int(2)");

                entity.Property(e => e._2)
                    .HasColumnName("2")
                    .HasColumnType("int(2)");

                entity.Property(e => e._20)
                    .HasColumnName("20")
                    .HasColumnType("int(2)");

                entity.Property(e => e._21)
                    .HasColumnName("21")
                    .HasColumnType("int(2)");

                entity.Property(e => e._22)
                    .HasColumnName("22")
                    .HasColumnType("int(2)");

                entity.Property(e => e._23)
                    .HasColumnName("23")
                    .HasColumnType("int(2)");

                entity.Property(e => e._24)
                    .HasColumnName("24")
                    .HasColumnType("int(2)");

                entity.Property(e => e._25)
                    .HasColumnName("25")
                    .HasColumnType("int(2)");

                entity.Property(e => e._26)
                    .HasColumnName("26")
                    .HasColumnType("int(2)");

                entity.Property(e => e._27)
                    .HasColumnName("27")
                    .HasColumnType("int(2)");

                entity.Property(e => e._28)
                    .HasColumnName("28")
                    .HasColumnType("int(2)");

                entity.Property(e => e._29)
                    .HasColumnName("29")
                    .HasColumnType("int(2)");

                entity.Property(e => e._3)
                    .HasColumnName("3")
                    .HasColumnType("int(2)");

                entity.Property(e => e._30)
                    .HasColumnName("30")
                    .HasColumnType("int(2)");

                entity.Property(e => e._31)
                    .HasColumnName("31")
                    .HasColumnType("int(2)");

                entity.Property(e => e._32)
                    .HasColumnName("32")
                    .HasColumnType("int(2)");

                entity.Property(e => e._33)
                    .HasColumnName("33")
                    .HasColumnType("int(2)");

                entity.Property(e => e._34)
                    .HasColumnName("34")
                    .HasColumnType("int(2)");

                entity.Property(e => e._35)
                    .HasColumnName("35")
                    .HasColumnType("int(2)");

                entity.Property(e => e._36)
                    .HasColumnName("36")
                    .HasColumnType("int(2)");

                entity.Property(e => e._37)
                    .HasColumnName("37")
                    .HasColumnType("int(2)");

                entity.Property(e => e._38)
                    .HasColumnName("38")
                    .HasColumnType("int(2)");

                entity.Property(e => e._39)
                    .HasColumnName("39")
                    .HasColumnType("int(2)");

                entity.Property(e => e._4)
                    .HasColumnName("4")
                    .HasColumnType("int(2)");

                entity.Property(e => e._40)
                    .HasColumnName("40")
                    .HasColumnType("int(2)");

                entity.Property(e => e._41)
                    .HasColumnName("41")
                    .HasColumnType("int(2)");

                entity.Property(e => e._42)
                    .HasColumnName("42")
                    .HasColumnType("int(2)");

                entity.Property(e => e._43)
                    .HasColumnName("43")
                    .HasColumnType("int(2)");

                entity.Property(e => e._44)
                    .HasColumnName("44")
                    .HasColumnType("int(2)");

                entity.Property(e => e._45)
                    .HasColumnName("45")
                    .HasColumnType("int(2)");

                entity.Property(e => e._46)
                    .HasColumnName("46")
                    .HasColumnType("int(2)");

                entity.Property(e => e._47)
                    .HasColumnName("47")
                    .HasColumnType("int(2)");

                entity.Property(e => e._48)
                    .HasColumnName("48")
                    .HasColumnType("int(2)");

                entity.Property(e => e._49)
                    .HasColumnName("49")
                    .HasColumnType("int(2)");

                entity.Property(e => e._5)
                    .HasColumnName("5")
                    .HasColumnType("int(2)");

                entity.Property(e => e._50)
                    .HasColumnName("50")
                    .HasColumnType("int(2)");

                entity.Property(e => e._51)
                    .HasColumnName("51")
                    .HasColumnType("int(2)");

                entity.Property(e => e._52)
                    .HasColumnName("52")
                    .HasColumnType("int(2)");

                entity.Property(e => e._53)
                    .HasColumnName("53")
                    .HasColumnType("int(2)");

                entity.Property(e => e._54)
                    .HasColumnName("54")
                    .HasColumnType("int(2)");

                entity.Property(e => e._55)
                    .HasColumnName("55")
                    .HasColumnType("int(2)");

                entity.Property(e => e._56)
                    .HasColumnName("56")
                    .HasColumnType("int(2)");

                entity.Property(e => e._57)
                    .HasColumnName("57")
                    .HasColumnType("int(2)");

                entity.Property(e => e._58)
                    .HasColumnName("58")
                    .HasColumnType("int(2)");

                entity.Property(e => e._59)
                    .HasColumnName("59")
                    .HasColumnType("int(2)");

                entity.Property(e => e._6)
                    .HasColumnName("6")
                    .HasColumnType("int(2)");

                entity.Property(e => e._60)
                    .HasColumnName("60")
                    .HasColumnType("int(2)");

                entity.Property(e => e._61)
                    .HasColumnName("61")
                    .HasColumnType("int(2)");

                entity.Property(e => e._62)
                    .HasColumnName("62")
                    .HasColumnType("int(2)");

                entity.Property(e => e._63)
                    .HasColumnName("63")
                    .HasColumnType("int(2)");

                entity.Property(e => e._64)
                    .HasColumnName("64")
                    .HasColumnType("int(2)");

                entity.Property(e => e._65)
                    .HasColumnName("65")
                    .HasColumnType("int(2)");

                entity.Property(e => e._66)
                    .HasColumnName("66")
                    .HasColumnType("int(2)");

                entity.Property(e => e._67)
                    .HasColumnName("67")
                    .HasColumnType("int(2)");

                entity.Property(e => e._68)
                    .HasColumnName("68")
                    .HasColumnType("int(2)");

                entity.Property(e => e._69)
                    .HasColumnName("69")
                    .HasColumnType("int(2)");

                entity.Property(e => e._7)
                    .HasColumnName("7")
                    .HasColumnType("int(2)");

                entity.Property(e => e._70)
                    .HasColumnName("70")
                    .HasColumnType("int(2)");

                entity.Property(e => e._71)
                    .HasColumnName("71")
                    .HasColumnType("int(2)");

                entity.Property(e => e._72)
                    .HasColumnName("72")
                    .HasColumnType("int(2)");

                entity.Property(e => e._73)
                    .HasColumnName("73")
                    .HasColumnType("int(2)");

                entity.Property(e => e._74)
                    .HasColumnName("74")
                    .HasColumnType("int(2)");

                entity.Property(e => e._75)
                    .HasColumnName("75")
                    .HasColumnType("int(2)");

                entity.Property(e => e._76)
                    .HasColumnName("76")
                    .HasColumnType("int(2)");

                entity.Property(e => e._77)
                    .HasColumnName("77")
                    .HasColumnType("int(2)");

                entity.Property(e => e._78)
                    .HasColumnName("78")
                    .HasColumnType("int(2)");

                entity.Property(e => e._79)
                    .HasColumnName("79")
                    .HasColumnType("int(2)");

                entity.Property(e => e._8)
                    .HasColumnName("8")
                    .HasColumnType("int(2)");

                entity.Property(e => e._80)
                    .HasColumnName("80")
                    .HasColumnType("int(2)");

                entity.Property(e => e._81)
                    .HasColumnName("81")
                    .HasColumnType("int(2)");

                entity.Property(e => e._82)
                    .HasColumnName("82")
                    .HasColumnType("int(2)");

                entity.Property(e => e._83)
                    .HasColumnName("83")
                    .HasColumnType("int(2)");

                entity.Property(e => e._84)
                    .HasColumnName("84")
                    .HasColumnType("int(2)");

                entity.Property(e => e._85)
                    .HasColumnName("85")
                    .HasColumnType("int(2)");

                entity.Property(e => e._86)
                    .HasColumnName("86")
                    .HasColumnType("int(2)");

                entity.Property(e => e._87)
                    .HasColumnName("87")
                    .HasColumnType("int(2)");

                entity.Property(e => e._88)
                    .HasColumnName("88")
                    .HasColumnType("int(2)");

                entity.Property(e => e._89)
                    .HasColumnName("89")
                    .HasColumnType("int(2)");

                entity.Property(e => e._9)
                    .HasColumnName("9")
                    .HasColumnType("int(2)");

                entity.Property(e => e._90)
                    .HasColumnName("90")
                    .HasColumnType("int(2)");

                entity.Property(e => e._91)
                    .HasColumnName("91")
                    .HasColumnType("int(2)");

                entity.Property(e => e._92)
                    .HasColumnName("92")
                    .HasColumnType("int(2)");

                entity.Property(e => e._93)
                    .HasColumnName("93")
                    .HasColumnType("int(2)");

                entity.Property(e => e._94)
                    .HasColumnName("94")
                    .HasColumnType("int(2)");

                entity.Property(e => e._95)
                    .HasColumnName("95")
                    .HasColumnType("int(2)");

                entity.Property(e => e._96)
                    .HasColumnName("96")
                    .HasColumnType("int(2)");

                entity.Property(e => e._97)
                    .HasColumnName("97")
                    .HasColumnType("int(2)");

                entity.Property(e => e._98)
                    .HasColumnName("98")
                    .HasColumnType("int(2)");

                entity.Property(e => e._99)
                    .HasColumnName("99")
                    .HasColumnType("int(2)");
            });

            modelBuilder.Entity<GioGiat>(entity =>
            {
                entity.HasKey(e => e.DiemId)
                    .HasName("PRIMARY");

                entity.ToTable("giogiat");

                entity.HasIndex(e => e.DiemId)
                    .HasName("DiemId");

                entity.Property(e => e.DiemId).HasMaxLength(255);
                entity.Property(e => e.RefDate)
                    .HasColumnName("RefDate")
                    .HasColumnType("date");

                entity.Property(e => e._1)
                    .HasColumnName("1")
                    .HasColumnType("int(2)");

                entity.Property(e => e._10)
                    .HasColumnName("10")
                    .HasColumnType("int(2)");

                entity.Property(e => e._100)
                    .HasColumnName("100")
                    .HasColumnType("int(2)");

                entity.Property(e => e._101)
                    .HasColumnName("101")
                    .HasColumnType("int(2)");

                entity.Property(e => e._102)
                    .HasColumnName("102")
                    .HasColumnType("int(2)");

                entity.Property(e => e._103)
                    .HasColumnName("103")
                    .HasColumnType("int(2)");

                entity.Property(e => e._104)
                    .HasColumnName("104")
                    .HasColumnType("int(2)");

                entity.Property(e => e._105)
                    .HasColumnName("105")
                    .HasColumnType("int(2)");

                entity.Property(e => e._106)
                    .HasColumnName("106")
                    .HasColumnType("int(2)");

                entity.Property(e => e._107)
                    .HasColumnName("107")
                    .HasColumnType("int(2)");

                entity.Property(e => e._108)
                    .HasColumnName("108")
                    .HasColumnType("int(2)");

                entity.Property(e => e._109)
                    .HasColumnName("109")
                    .HasColumnType("int(2)");

                entity.Property(e => e._11)
                    .HasColumnName("11")
                    .HasColumnType("int(2)");

                entity.Property(e => e._110)
                    .HasColumnName("110")
                    .HasColumnType("int(2)");

                entity.Property(e => e._111)
                    .HasColumnName("111")
                    .HasColumnType("int(2)");

                entity.Property(e => e._112)
                    .HasColumnName("112")
                    .HasColumnType("int(2)");

                entity.Property(e => e._113)
                    .HasColumnName("113")
                    .HasColumnType("int(2)");

                entity.Property(e => e._114)
                    .HasColumnName("114")
                    .HasColumnType("int(2)");

                entity.Property(e => e._115)
                    .HasColumnName("115")
                    .HasColumnType("int(2)");

                entity.Property(e => e._116)
                    .HasColumnName("116")
                    .HasColumnType("int(2)");

                entity.Property(e => e._117)
                    .HasColumnName("117")
                    .HasColumnType("int(2)");

                entity.Property(e => e._118)
                    .HasColumnName("118")
                    .HasColumnType("int(2)");

                entity.Property(e => e._119)
                    .HasColumnName("119")
                    .HasColumnType("int(2)");

                entity.Property(e => e._12)
                    .HasColumnName("12")
                    .HasColumnType("int(2)");

                entity.Property(e => e._120)
                    .HasColumnName("120")
                    .HasColumnType("int(2)");

                entity.Property(e => e._13)
                    .HasColumnName("13")
                    .HasColumnType("int(2)");

                entity.Property(e => e._14)
                    .HasColumnName("14")
                    .HasColumnType("int(2)");

                entity.Property(e => e._15)
                    .HasColumnName("15")
                    .HasColumnType("int(2)");

                entity.Property(e => e._16)
                    .HasColumnName("16")
                    .HasColumnType("int(2)");

                entity.Property(e => e._17)
                    .HasColumnName("17")
                    .HasColumnType("int(2)");

                entity.Property(e => e._18)
                    .HasColumnName("18")
                    .HasColumnType("int(2)");

                entity.Property(e => e._19)
                    .HasColumnName("19")
                    .HasColumnType("int(2)");

                entity.Property(e => e._2)
                    .HasColumnName("2")
                    .HasColumnType("int(2)");

                entity.Property(e => e._20)
                    .HasColumnName("20")
                    .HasColumnType("int(2)");

                entity.Property(e => e._21)
                    .HasColumnName("21")
                    .HasColumnType("int(2)");

                entity.Property(e => e._22)
                    .HasColumnName("22")
                    .HasColumnType("int(2)");

                entity.Property(e => e._23)
                    .HasColumnName("23")
                    .HasColumnType("int(2)");

                entity.Property(e => e._24)
                    .HasColumnName("24")
                    .HasColumnType("int(2)");

                entity.Property(e => e._25)
                    .HasColumnName("25")
                    .HasColumnType("int(2)");

                entity.Property(e => e._26)
                    .HasColumnName("26")
                    .HasColumnType("int(2)");

                entity.Property(e => e._27)
                    .HasColumnName("27")
                    .HasColumnType("int(2)");

                entity.Property(e => e._28)
                    .HasColumnName("28")
                    .HasColumnType("int(2)");

                entity.Property(e => e._29)
                    .HasColumnName("29")
                    .HasColumnType("int(2)");

                entity.Property(e => e._3)
                    .HasColumnName("3")
                    .HasColumnType("int(2)");

                entity.Property(e => e._30)
                    .HasColumnName("30")
                    .HasColumnType("int(2)");

                entity.Property(e => e._31)
                    .HasColumnName("31")
                    .HasColumnType("int(2)");

                entity.Property(e => e._32)
                    .HasColumnName("32")
                    .HasColumnType("int(2)");

                entity.Property(e => e._33)
                    .HasColumnName("33")
                    .HasColumnType("int(2)");

                entity.Property(e => e._34)
                    .HasColumnName("34")
                    .HasColumnType("int(2)");

                entity.Property(e => e._35)
                    .HasColumnName("35")
                    .HasColumnType("int(2)");

                entity.Property(e => e._36)
                    .HasColumnName("36")
                    .HasColumnType("int(2)");

                entity.Property(e => e._37)
                    .HasColumnName("37")
                    .HasColumnType("int(2)");

                entity.Property(e => e._38)
                    .HasColumnName("38")
                    .HasColumnType("int(2)");

                entity.Property(e => e._39)
                    .HasColumnName("39")
                    .HasColumnType("int(2)");

                entity.Property(e => e._4)
                    .HasColumnName("4")
                    .HasColumnType("int(2)");

                entity.Property(e => e._40)
                    .HasColumnName("40")
                    .HasColumnType("int(2)");

                entity.Property(e => e._41)
                    .HasColumnName("41")
                    .HasColumnType("int(2)");

                entity.Property(e => e._42)
                    .HasColumnName("42")
                    .HasColumnType("int(2)");

                entity.Property(e => e._43)
                    .HasColumnName("43")
                    .HasColumnType("int(2)");

                entity.Property(e => e._44)
                    .HasColumnName("44")
                    .HasColumnType("int(2)");

                entity.Property(e => e._45)
                    .HasColumnName("45")
                    .HasColumnType("int(2)");

                entity.Property(e => e._46)
                    .HasColumnName("46")
                    .HasColumnType("int(2)");

                entity.Property(e => e._47)
                    .HasColumnName("47")
                    .HasColumnType("int(2)");

                entity.Property(e => e._48)
                    .HasColumnName("48")
                    .HasColumnType("int(2)");

                entity.Property(e => e._49)
                    .HasColumnName("49")
                    .HasColumnType("int(2)");

                entity.Property(e => e._5)
                    .HasColumnName("5")
                    .HasColumnType("int(2)");

                entity.Property(e => e._50)
                    .HasColumnName("50")
                    .HasColumnType("int(2)");

                entity.Property(e => e._51)
                    .HasColumnName("51")
                    .HasColumnType("int(2)");

                entity.Property(e => e._52)
                    .HasColumnName("52")
                    .HasColumnType("int(2)");

                entity.Property(e => e._53)
                    .HasColumnName("53")
                    .HasColumnType("int(2)");

                entity.Property(e => e._54)
                    .HasColumnName("54")
                    .HasColumnType("int(2)");

                entity.Property(e => e._55)
                    .HasColumnName("55")
                    .HasColumnType("int(2)");

                entity.Property(e => e._56)
                    .HasColumnName("56")
                    .HasColumnType("int(2)");

                entity.Property(e => e._57)
                    .HasColumnName("57")
                    .HasColumnType("int(2)");

                entity.Property(e => e._58)
                    .HasColumnName("58")
                    .HasColumnType("int(2)");

                entity.Property(e => e._59)
                    .HasColumnName("59")
                    .HasColumnType("int(2)");

                entity.Property(e => e._6)
                    .HasColumnName("6")
                    .HasColumnType("int(2)");

                entity.Property(e => e._60)
                    .HasColumnName("60")
                    .HasColumnType("int(2)");

                entity.Property(e => e._61)
                    .HasColumnName("61")
                    .HasColumnType("int(2)");

                entity.Property(e => e._62)
                    .HasColumnName("62")
                    .HasColumnType("int(2)");

                entity.Property(e => e._63)
                    .HasColumnName("63")
                    .HasColumnType("int(2)");

                entity.Property(e => e._64)
                    .HasColumnName("64")
                    .HasColumnType("int(2)");

                entity.Property(e => e._65)
                    .HasColumnName("65")
                    .HasColumnType("int(2)");

                entity.Property(e => e._66)
                    .HasColumnName("66")
                    .HasColumnType("int(2)");

                entity.Property(e => e._67)
                    .HasColumnName("67")
                    .HasColumnType("int(2)");

                entity.Property(e => e._68)
                    .HasColumnName("68")
                    .HasColumnType("int(2)");

                entity.Property(e => e._69)
                    .HasColumnName("69")
                    .HasColumnType("int(2)");

                entity.Property(e => e._7)
                    .HasColumnName("7")
                    .HasColumnType("int(2)");

                entity.Property(e => e._70)
                    .HasColumnName("70")
                    .HasColumnType("int(2)");

                entity.Property(e => e._71)
                    .HasColumnName("71")
                    .HasColumnType("int(2)");

                entity.Property(e => e._72)
                    .HasColumnName("72")
                    .HasColumnType("int(2)");

                entity.Property(e => e._73)
                    .HasColumnName("73")
                    .HasColumnType("int(2)");

                entity.Property(e => e._74)
                    .HasColumnName("74")
                    .HasColumnType("int(2)");

                entity.Property(e => e._75)
                    .HasColumnName("75")
                    .HasColumnType("int(2)");

                entity.Property(e => e._76)
                    .HasColumnName("76")
                    .HasColumnType("int(2)");

                entity.Property(e => e._77)
                    .HasColumnName("77")
                    .HasColumnType("int(2)");

                entity.Property(e => e._78)
                    .HasColumnName("78")
                    .HasColumnType("int(2)");

                entity.Property(e => e._79)
                    .HasColumnName("79")
                    .HasColumnType("int(2)");

                entity.Property(e => e._8)
                    .HasColumnName("8")
                    .HasColumnType("int(2)");

                entity.Property(e => e._80)
                    .HasColumnName("80")
                    .HasColumnType("int(2)");

                entity.Property(e => e._81)
                    .HasColumnName("81")
                    .HasColumnType("int(2)");

                entity.Property(e => e._82)
                    .HasColumnName("82")
                    .HasColumnType("int(2)");

                entity.Property(e => e._83)
                    .HasColumnName("83")
                    .HasColumnType("int(2)");

                entity.Property(e => e._84)
                    .HasColumnName("84")
                    .HasColumnType("int(2)");

                entity.Property(e => e._85)
                    .HasColumnName("85")
                    .HasColumnType("int(2)");

                entity.Property(e => e._86)
                    .HasColumnName("86")
                    .HasColumnType("int(2)");

                entity.Property(e => e._87)
                    .HasColumnName("87")
                    .HasColumnType("int(2)");

                entity.Property(e => e._88)
                    .HasColumnName("88")
                    .HasColumnType("int(2)");

                entity.Property(e => e._89)
                    .HasColumnName("89")
                    .HasColumnType("int(2)");

                entity.Property(e => e._9)
                    .HasColumnName("9")
                    .HasColumnType("int(2)");

                entity.Property(e => e._90)
                    .HasColumnName("90")
                    .HasColumnType("int(2)");

                entity.Property(e => e._91)
                    .HasColumnName("91")
                    .HasColumnType("int(2)");

                entity.Property(e => e._92)
                    .HasColumnName("92")
                    .HasColumnType("int(2)");

                entity.Property(e => e._93)
                    .HasColumnName("93")
                    .HasColumnType("int(2)");

                entity.Property(e => e._94)
                    .HasColumnName("94")
                    .HasColumnType("int(2)");

                entity.Property(e => e._95)
                    .HasColumnName("95")
                    .HasColumnType("int(2)");

                entity.Property(e => e._96)
                    .HasColumnName("96")
                    .HasColumnType("int(2)");

                entity.Property(e => e._97)
                    .HasColumnName("97")
                    .HasColumnType("int(2)");

                entity.Property(e => e._98)
                    .HasColumnName("98")
                    .HasColumnType("int(2)");

                entity.Property(e => e._99)
                    .HasColumnName("99")
                    .HasColumnType("int(2)");
            });

            modelBuilder.Entity<ThoiTiet>(entity =>
            {
                entity.HasKey(e => e.DiemId)
                    .HasName("PRIMARY");

                entity.ToTable("thoitiet");

                entity.HasIndex(e => e.DiemId)
                    .HasName("DiemId");

                entity.Property(e => e.DiemId).HasMaxLength(255);
                entity.Property(e => e.RefDate)
                    .HasColumnName("RefDate");

                entity.Property(e => e._1)
                    .HasColumnName("1")
                    .HasColumnType("varchar(10)");
                

                entity.Property(e => e._10)
                    .HasColumnName("10")
                    .HasColumnType("varchar(10)");


                entity.Property(e => e._100)
                    .HasColumnName("100")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._101)
                    .HasColumnName("101")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._102)
                    .HasColumnName("102")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._103)
                    .HasColumnName("103")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._104)
                    .HasColumnName("104")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._105)
                    .HasColumnName("105")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._106)
                    .HasColumnName("106")
                    .HasColumnType("varchar");

                entity.Property(e => e._107)
                    .HasColumnName("107")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._108)
                    .HasColumnName("108")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._109)
                    .HasColumnName("109")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._11)
                    .HasColumnName("11")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._110)
                    .HasColumnName("110")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._111)
                    .HasColumnName("111")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._112)
                    .HasColumnName("112")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._113)
                    .HasColumnName("113")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._114)
                    .HasColumnName("114")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._115)
                    .HasColumnName("115")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._116)
                    .HasColumnName("116")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._117)
                    .HasColumnName("117")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._118)
                    .HasColumnName("118")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._119)
                    .HasColumnName("119")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._12)
                    .HasColumnName("12")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._120)
                    .HasColumnName("120")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._13)
                    .HasColumnName("13")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._14)
                    .HasColumnName("14")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._15)
                    .HasColumnName("15")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._16)
                    .HasColumnName("16")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._17)
                    .HasColumnName("17")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._18)
                    .HasColumnName("18")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._19)
                    .HasColumnName("19")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._2)
                    .HasColumnName("2")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._20)
                    .HasColumnName("20")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._21)
                    .HasColumnName("21")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._22)
                    .HasColumnName("22")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._23)
                    .HasColumnName("23")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._24)
                    .HasColumnName("24")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._25)
                    .HasColumnName("25")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._26)
                    .HasColumnName("26")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._27)
                    .HasColumnName("27")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._28)
                    .HasColumnName("28")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._29)
                    .HasColumnName("29")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._3)
                    .HasColumnName("3")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._30)
                    .HasColumnName("30")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._31)
                    .HasColumnName("31")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._32)
                    .HasColumnName("32")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._33)
                    .HasColumnName("33")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._34)
                    .HasColumnName("34")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._35)
                    .HasColumnName("35")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._36)
                    .HasColumnName("36")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._37)
                    .HasColumnName("37")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._38)
                    .HasColumnName("38")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._39)
                    .HasColumnName("39")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._4)
                    .HasColumnName("4")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._40)
                    .HasColumnName("40")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._41)
                    .HasColumnName("41")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._42)
                    .HasColumnName("42")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._43)
                    .HasColumnName("43")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._44)
                    .HasColumnName("44")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._45)
                    .HasColumnName("45")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._46)
                    .HasColumnName("46")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._47)
                    .HasColumnName("47")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._48)
                    .HasColumnName("48")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._49)
                    .HasColumnName("49")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._5)
                    .HasColumnName("5")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._50)
                    .HasColumnName("50")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._51)
                    .HasColumnName("51")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._52)
                    .HasColumnName("52")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._53)
                    .HasColumnName("53")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._54)
                    .HasColumnName("54")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._55)
                    .HasColumnName("55")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._56)
                    .HasColumnName("56")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._57)
                    .HasColumnName("57")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._58)
                    .HasColumnName("58")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._59)
                    .HasColumnName("59")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._6)
                    .HasColumnName("6")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._60)
                    .HasColumnName("60")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._61)
                    .HasColumnName("61")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._62)
                    .HasColumnName("62")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._63)
                    .HasColumnName("63")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._64)
                    .HasColumnName("64")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._65)
                    .HasColumnName("65")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._66)
                    .HasColumnName("66")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._67)
                    .HasColumnName("67")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._68)
                    .HasColumnName("68")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._69)
                    .HasColumnName("69")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._7)
                    .HasColumnName("7")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._70)
                    .HasColumnName("70")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._71)
                    .HasColumnName("71")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._72)
                    .HasColumnName("72")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._73)
                    .HasColumnName("73")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._74)
                    .HasColumnName("74")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._75)
                    .HasColumnName("75")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._76)
                    .HasColumnName("76")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._77)
                    .HasColumnName("77")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._78)
                    .HasColumnName("78")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._79)
                    .HasColumnName("79")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._8)
                    .HasColumnName("8")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._80)
                    .HasColumnName("80")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._81)
                    .HasColumnName("81")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._82)
                    .HasColumnName("82")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._83)
                    .HasColumnName("83")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._84)
                    .HasColumnName("84")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._85)
                    .HasColumnName("85")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._86)
                    .HasColumnName("86")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._87)
                    .HasColumnName("87")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._88)
                    .HasColumnName("88")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._89)
                    .HasColumnName("89")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._9)
                    .HasColumnName("9")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._90)
                    .HasColumnName("90")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._91)
                    .HasColumnName("91")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._92)
                    .HasColumnName("92")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._93)
                    .HasColumnName("93")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._94)
                    .HasColumnName("94")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._95)
                    .HasColumnName("95")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._96)
                    .HasColumnName("96")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._97)
                    .HasColumnName("97")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._98)
                    .HasColumnName("98")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e._99)
                    .HasColumnName("99")
                    .HasColumnType("varchar(10)");
            });

            modelBuilder.Entity<Thoigian>(entity =>
            {
                entity.ToTable("thoigian");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MoTa).HasMaxLength(255);
            });

            modelBuilder.Entity<HuongGio>(entity =>
            {
                entity.HasKey(e => e.DiemId)
                    .HasName("PRIMARY");

                entity.ToTable("huonggio");

                entity.HasIndex(e => e.DiemId)
                    .HasName("DiemId");

                entity.Property(e => e.DiemId).HasMaxLength(255);
                entity.Property(e => e.RefDate)
                    .HasColumnName("RefDate");

                entity.Property(e => e._1)
                    .HasColumnName("1");

                entity.Property(e => e._10)
                    .HasColumnName("10");

                entity.Property(e => e._100)
                    .HasColumnName("100");

                entity.Property(e => e._101)
                    .HasColumnName("101");

                entity.Property(e => e._102)
                    .HasColumnName("102");

                entity.Property(e => e._103)
                    .HasColumnName("103");

                entity.Property(e => e._104)
                    .HasColumnName("104");

                entity.Property(e => e._105)
                    .HasColumnName("105");

                entity.Property(e => e._106)
                    .HasColumnName("106");

                entity.Property(e => e._107)
                    .HasColumnName("107");

                entity.Property(e => e._108)
                    .HasColumnName("108");

                entity.Property(e => e._109)
                    .HasColumnName("109");

                entity.Property(e => e._11)
                    .HasColumnName("11");

                entity.Property(e => e._110)
                    .HasColumnName("110");

                entity.Property(e => e._111)
                    .HasColumnName("111");

                entity.Property(e => e._112)
                    .HasColumnName("112");

                entity.Property(e => e._113)
                    .HasColumnName("113");

                entity.Property(e => e._114)
                    .HasColumnName("114");

                entity.Property(e => e._115)
                    .HasColumnName("115");

                entity.Property(e => e._116)
                    .HasColumnName("116");

                entity.Property(e => e._117)
                    .HasColumnName("117");

                entity.Property(e => e._118)
                    .HasColumnName("118");

                entity.Property(e => e._119)
                    .HasColumnName("119");

                entity.Property(e => e._12)
                    .HasColumnName("12");

                entity.Property(e => e._120)
                    .HasColumnName("120");

                entity.Property(e => e._13)
                    .HasColumnName("13");

                entity.Property(e => e._14)
                    .HasColumnName("14");

                entity.Property(e => e._15)
                    .HasColumnName("15");

                entity.Property(e => e._16)
                    .HasColumnName("16");

                entity.Property(e => e._17)
                    .HasColumnName("17");

                entity.Property(e => e._18)
                    .HasColumnName("18");

                entity.Property(e => e._19)
                    .HasColumnName("19");

                entity.Property(e => e._2)
                    .HasColumnName("2");

                entity.Property(e => e._20)
                    .HasColumnName("20");

                entity.Property(e => e._21)
                    .HasColumnName("21");

                entity.Property(e => e._22)
                    .HasColumnName("22");

                entity.Property(e => e._23)
                    .HasColumnName("23");

                entity.Property(e => e._24)
                    .HasColumnName("24");

                entity.Property(e => e._25)
                    .HasColumnName("25");

                entity.Property(e => e._26)
                    .HasColumnName("26");

                entity.Property(e => e._27)
                    .HasColumnName("27");

                entity.Property(e => e._28)
                    .HasColumnName("28");

                entity.Property(e => e._29)
                    .HasColumnName("29");

                entity.Property(e => e._3)
                    .HasColumnName("3");

                entity.Property(e => e._30)
                    .HasColumnName("30");

                entity.Property(e => e._31)
                    .HasColumnName("31");

                entity.Property(e => e._32)
                    .HasColumnName("32");

                entity.Property(e => e._33)
                    .HasColumnName("33");

                entity.Property(e => e._34)
                    .HasColumnName("34");

                entity.Property(e => e._35)
                    .HasColumnName("35");

                entity.Property(e => e._36)
                    .HasColumnName("36");

                entity.Property(e => e._37)
                    .HasColumnName("37");

                entity.Property(e => e._38)
                    .HasColumnName("38");

                entity.Property(e => e._39)
                    .HasColumnName("39");

                entity.Property(e => e._4)
                    .HasColumnName("4");

                entity.Property(e => e._40)
                    .HasColumnName("40");

                entity.Property(e => e._41)
                    .HasColumnName("41");

                entity.Property(e => e._42)
                    .HasColumnName("42");

                entity.Property(e => e._43)
                    .HasColumnName("43");

                entity.Property(e => e._44)
                    .HasColumnName("44");

                entity.Property(e => e._45)
                    .HasColumnName("45");

                entity.Property(e => e._46)
                    .HasColumnName("46");

                entity.Property(e => e._47)
                    .HasColumnName("47");

                entity.Property(e => e._48)
                    .HasColumnName("48");

                entity.Property(e => e._49)
                    .HasColumnName("49");

                entity.Property(e => e._5)
                    .HasColumnName("5");

                entity.Property(e => e._50)
                    .HasColumnName("50");

                entity.Property(e => e._51)
                    .HasColumnName("51");

                entity.Property(e => e._52)
                    .HasColumnName("52");

                entity.Property(e => e._53)
                    .HasColumnName("53");

                entity.Property(e => e._54)
                    .HasColumnName("54");

                entity.Property(e => e._55)
                    .HasColumnName("55");

                entity.Property(e => e._56)
                    .HasColumnName("56");

                entity.Property(e => e._57)
                    .HasColumnName("57");

                entity.Property(e => e._58)
                    .HasColumnName("58");

                entity.Property(e => e._59)
                    .HasColumnName("59");

                entity.Property(e => e._6)
                    .HasColumnName("6");

                entity.Property(e => e._60)
                    .HasColumnName("60");

                entity.Property(e => e._61)
                    .HasColumnName("61");

                entity.Property(e => e._62)
                    .HasColumnName("62");

                entity.Property(e => e._63)
                    .HasColumnName("63");

                entity.Property(e => e._64)
                    .HasColumnName("64");

                entity.Property(e => e._65)
                    .HasColumnName("65");

                entity.Property(e => e._66)
                    .HasColumnName("66");

                entity.Property(e => e._67)
                    .HasColumnName("67");

                entity.Property(e => e._68)
                    .HasColumnName("68");

                entity.Property(e => e._69)
                    .HasColumnName("69");

                entity.Property(e => e._7)
                    .HasColumnName("7");

                entity.Property(e => e._70)
                    .HasColumnName("70");

                entity.Property(e => e._71)
                    .HasColumnName("71");

                entity.Property(e => e._72)
                    .HasColumnName("72");

                entity.Property(e => e._73)
                    .HasColumnName("73");

                entity.Property(e => e._74)
                    .HasColumnName("74");

                entity.Property(e => e._75)
                    .HasColumnName("75");

                entity.Property(e => e._76)
                    .HasColumnName("76");

                entity.Property(e => e._77)
                    .HasColumnName("77");

                entity.Property(e => e._78)
                    .HasColumnName("78");

                entity.Property(e => e._79)
                    .HasColumnName("79");

                entity.Property(e => e._8)
                    .HasColumnName("8");

                entity.Property(e => e._80)
                    .HasColumnName("80");

                entity.Property(e => e._81)
                    .HasColumnName("81");

                entity.Property(e => e._82)
                    .HasColumnName("82");

                entity.Property(e => e._83)
                    .HasColumnName("83");

                entity.Property(e => e._84)
                    .HasColumnName("84");

                entity.Property(e => e._85)
                    .HasColumnName("85");

                entity.Property(e => e._86)
                    .HasColumnName("86");

                entity.Property(e => e._87)
                    .HasColumnName("87");

                entity.Property(e => e._88)
                    .HasColumnName("88");

                entity.Property(e => e._89)
                    .HasColumnName("89");

                entity.Property(e => e._9)
                    .HasColumnName("9");

                entity.Property(e => e._90)
                    .HasColumnName("90");

                entity.Property(e => e._91)
                    .HasColumnName("91");

                entity.Property(e => e._92)
                    .HasColumnName("92");

                entity.Property(e => e._93)
                    .HasColumnName("93");

                entity.Property(e => e._94)
                    .HasColumnName("94");

                entity.Property(e => e._95)
                    .HasColumnName("95");

                entity.Property(e => e._96)
                    .HasColumnName("96");

                entity.Property(e => e._97)
                    .HasColumnName("97");

                entity.Property(e => e._98)
                    .HasColumnName("98");

                entity.Property(e => e._99)
                    .HasColumnName("99");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
