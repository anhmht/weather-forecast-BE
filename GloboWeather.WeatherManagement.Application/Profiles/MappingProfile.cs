using AutoMapper;
using GloboWeather.WeatherManagement.Application.Features.Categories.Commands.CreateCategory;
using GloboWeather.WeatherManagement.Application.Features.Categories.Queries.GetCategoryDetail;
using GloboWeather.WeatherManagement.Application.Features.Categories.Queries.GetCategoryList;
using GloboWeather.WeatherManagement.Application.Features.Events.Commands.CreateEvent;
using GloboWeather.WeatherManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Update;

namespace GloboWeather.WeatherManagement.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, CreateEventCommand>().ReverseMap();
            CreateMap<Event, EventListVm>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<Event, UpdateEventCommand>().ReverseMap();
            CreateMap<Event, EventDetailVm>();
            
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryCommand>().ReverseMap();
            CreateMap<Category, CategoriesListVm>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>();
            CreateMap<Category, CategoryDetailVm>().ReverseMap();
        }
    }
}