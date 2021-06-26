using AutoMapper;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Application.Models.Weather.RainAmount;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindDirection;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindLevel;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManagement.Weather.weathercontext;

namespace GloboWeather.WeatherManagement.Weather.Profiles
{
    
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<DiemDuBaoResponse, Diemdubao>().ReverseMap();
            CreateMap<TemperatureResponse, Nhietdo>().ReverseMap();
            CreateMap<HumidityResponse, DoAmTB>().ReverseMap();
            CreateMap<WindLevelResponse, GioGiat>().ReverseMap();
            CreateMap<WindSpeedResponse, TocDoGio>().ReverseMap();
            CreateMap<WeatherResponse, ThoiTiet>().ReverseMap();
            CreateMap<AmountOfRainResponse, AmountOfRain>().ReverseMap();
            CreateMap<WinLevelResponse, GioGiat>().ReverseMap();
            CreateMap<RainAmountResponse, AmountOfRain>().ReverseMap();
            CreateMap<WindDirectionResponse, HuongGio>().ReverseMap();
            CreateMap<BaseModelWeather, HumidityResponse>().ReverseMap();
            CreateMap<BaseModelWeather, AmountOfRainResponse>().ReverseMap();
            CreateMap<BaseModelWeather, RainAmountResponse>().ReverseMap();
            CreateMap<BaseModelWeather, TemperatureResponse>().ReverseMap();
            CreateMap<BaseModelWeather, WindLevelResponse>().ReverseMap();
            CreateMap<BaseModelWeather, WinLevelResponse>().ReverseMap();
            CreateMap<BaseModelWeather, WindSpeedResponse>().ReverseMap();
            CreateMap<BaseModelWeather, HumidityResponse>().ReverseMap();


            CreateMap<Capgio, WindRank>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.WindSpeed,
                    opt => opt.MapFrom(src => src.WindMS))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Color,
                    opt => opt.MapFrom(src => src.Color))
                .ForMember(dest => dest.Wave,
                    opt => opt.MapFrom(src => src.WaveM));

        }
    }
}