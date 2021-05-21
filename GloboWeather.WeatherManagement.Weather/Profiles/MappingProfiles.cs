using AutoMapper;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Weather.weathercontext;

namespace GloboWeather.WeatherManagement.Weather.Profiles
{
    
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<DiemDuBaoResponse, Diemdubao>().ReverseMap();
            CreateMap<NhietDoResponse, Nhietdo>().ReverseMap();
        }
    }
}