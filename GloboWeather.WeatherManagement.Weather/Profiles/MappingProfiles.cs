using AutoMapper;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Application.Models.Weather.RainAmount;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindDirection;
using GloboWeather.WeatherManagement.Application.Models.Weather.WindLevel;
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
        }
    }
}