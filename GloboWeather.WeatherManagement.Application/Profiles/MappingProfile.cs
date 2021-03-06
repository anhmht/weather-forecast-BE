using AutoMapper;
using GloboWeather.WeatherManagement.Application.Features.Categories.Commands.CreateCategory;
using GloboWeather.WeatherManagement.Application.Features.Categories.Commands.UpdateCategory;
using GloboWeather.WeatherManagement.Application.Features.Categories.Queries.GetCategoryDetail;
using GloboWeather.WeatherManagement.Application.Features.Categories.Queries.GetCategoryList;
using GloboWeather.WeatherManagement.Application.Features.Commons.Commands.CreateStatus;
using GloboWeather.WeatherManagement.Application.Features.Commons.Queries;
using GloboWeather.WeatherManagement.Application.Features.Events.Commands.CreateEvent;
using GloboWeather.WeatherManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboWeather.WeatherManagement.Application.Features.HydrologicalForeCasts.Import;
using GloboWeather.WeatherManagement.Application.Features.Hydrologicals.Import;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenario;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenario;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioDetail;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportSingleStation;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInformation;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Application.Features.Meteorologicals.Import;
using GloboWeather.WeatherManagement.Application.Features.RainQuantities.Import;
using GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.CreateWeatherState;
using GloboWeather.WeatherManagement.Application.Features.WeatherStates.Queries.GetWeatherStateDetail;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListWithContent;
using GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.CreateExtremePhenomenon;
using GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.ExtremePhenomenonDetail;
using GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.SearchExtremePhenomenonDetail;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioActionDetail;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListBy;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.CreatePost;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentDetailForApproval;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentsForApproval;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetail;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetailForApproval;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostList;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostsForApproval;
using GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Queries.GetListSocialNotification;
using GloboWeather.WeatherManagement.Domain.Entities.Social;

namespace GloboWeather.WeatherManagement.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateEventCommand, Event>();
            CreateMap<Event, EventListVm>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<Event, UpdateEventCommand>().ReverseMap();
            CreateMap<Event, EventDetailVm>();
            CreateMap<Event, EventListCateStatusVm>();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryCommand>().ReverseMap();
            CreateMap<Category, CategoriesListVm>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>();
            CreateMap<Category, CategoryDetailVm>().ReverseMap();

            CreateMap<Status, StatusDto>().ReverseMap();
            CreateMap<Status, CreateStatusCommand>().ReverseMap();
            CreateMap<Status, CreateCategoryDto>().ReverseMap();
            CreateMap<Status, StatusesListVm>().ReverseMap();
            CreateMap<ImportWeatherInformationDto, WeatherInformation>()
                .ForMember(dest => dest.StationId,
                    opt => opt.MapFrom(src => src.DiaDiemId))
                .ForMember(dest => dest.RefDate,
                    opt => opt.MapFrom(src => src.NgayGio))
                .ForMember(dest => dest.Humidity,
                    opt => opt.MapFrom(src => src.DoAm))
                .ForMember(dest => dest.WindLevel,
                    opt => opt.MapFrom(src => src.GioGiat))
                .ForMember(dest => dest.WindDirection,
                    opt => opt.MapFrom(src => src.HuongGio))
                .ForMember(dest => dest.WindSpeed,
                    opt => opt.MapFrom(src => src.TocDoGio))
                .ForMember(dest => dest.RainAmount,
                    opt => opt.MapFrom(src => src.LuongMua))
                .ForMember(dest => dest.Temperature,
                    opt => opt.MapFrom(src => src.NhietDo))
                .ForMember(dest => dest.Weather,
                    opt => opt.MapFrom(src => src.ThoiTiet));

            CreateMap<Scenario, CreateScenarioCommand>().ReverseMap();
            CreateMap<Scenario, UpdateScenarioCommand>().ReverseMap();
            CreateMap<Scenario, ScenarioDetailVm>().ReverseMap();
            CreateMap<UpdateCategoryCommand, Category>().ReverseMap();

            CreateMap<ImportSingleStationDto, WeatherInformation>()
                .ForMember(dest => dest.RefDate,
                    opt => opt.MapFrom(src => src.NgayGio))
                .ForMember(dest => dest.Humidity,
                    opt => opt.MapFrom(src => src.DoAm))
                .ForMember(dest => dest.WindLevel,
                    opt => opt.MapFrom(src => src.GioGiat))
                .ForMember(dest => dest.WindDirection,
                    opt => opt.MapFrom(src => src.HuongGio))
                .ForMember(dest => dest.WindSpeed,
                    opt => opt.MapFrom(src => src.TocDoGio))
                .ForMember(dest => dest.RainAmount,
                    opt => opt.MapFrom(src => src.LuongMua))
                .ForMember(dest => dest.Temperature,
                    opt => opt.MapFrom(src => src.NhietDo))
                .ForMember(dest => dest.Weather,
                    opt => opt.MapFrom(src => src.ThoiTiet));

            CreateMap<ImportHydrologicalDto, Hydrological>().ReverseMap();
            CreateMap<ImportHydrologicalForeCastDto, HydrologicalForeCast>().ReverseMap();
            CreateMap<ImportRainQuantityDto, RainQuantity>().ReverseMap();
            CreateMap<ImportMeteorologicalDto, Meteorological>().ReverseMap();
            CreateMap<CreateWeatherStateCommand, WeatherState>().ReverseMap();
            CreateMap<WeatherState, WeatherStateDetailVm>().ReverseMap();
            CreateMap<Event, EventListWithContentVm>();
            CreateMap<CreateExtremePhenomenonCommand, ExtremePhenomenon>();
            CreateMap<ExtremePhenomenon, ExtremePhenomenonDetailVm>();
            CreateMap<ExtremePhenomenon, SearchExtremePhenomenonDetailVm>();
            CreateMap<CreateScenarioActionCommand, ScenarioAction>();
            CreateMap<CreateScenarioActionDetailDto, ScenarioActionDetail>();
            CreateMap<UpdateScenarioActionDetailDto, ScenarioActionDetail>();
            CreateMap<ScenarioAction, ScenarioActionDetailVm>();
            CreateMap<ScenarioActionDetail, ScenarioActionDetailDto>();
            CreateMap<EventListVm, EventListCateStatusVm>();
            CreateMap<CreatePostCommand, Post>();
            CreateMap<Post, PostVm>();
            CreateMap<Comment, CommentVm>();
            CreateMap<Post, GetPostDetailResponse>();
            CreateMap<Post, PostForApprovalVm>();
            CreateMap<Comment, CommentForApprovalVm>();
            CreateMap<Post, GetPostDetailForApprovalResponse>();
            CreateMap<Comment, GetCommentDetailForApprovalResponse>();
            CreateMap<SocialNotification, SocialNotificationVm>();
        }
    }
}