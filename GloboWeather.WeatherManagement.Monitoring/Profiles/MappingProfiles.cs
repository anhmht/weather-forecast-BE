using AutoMapper;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Monitoring.MonitoringEntities;

namespace GloboWeather.WeatherManagement.Monitoring.Profiles
{
    
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<TramKttv, TramKttvResponse>().ReverseMap();
            CreateMap<Hydrological, Domain.Entities.Hydrological>()
                .ForMember(dest => dest.Accumulated,
                    opt => opt.MapFrom(src => src.ZLuyKe));
            CreateMap<Rain, Domain.Entities.RainQuantity>()
                .ForMember(dest => dest.RefDate,
                    opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.Quality));
            CreateMap<Meteorological, Domain.Entities.Meteorological>().ReverseMap();
        }   
    }
}