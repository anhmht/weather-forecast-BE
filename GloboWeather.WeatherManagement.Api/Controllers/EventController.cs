using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Events.Commands.CreateEvent;
using GloboWeather.WeatherManagement.Application.Features.Events.Commands.DeleteEvent;
using GloboWeather.WeatherManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListBy;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListWithContent;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsMostView;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GetAllEvents", Name = nameof(GetAllEvents))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<GetEventsListResponse>> GetAllEvents([FromBody] GetEventsListQuery query)
        {
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }

        [HttpGet("{id}", Name = "GetEventById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EventDetailVm>> GetEventById(Guid id)
        {
            var getEventDetailQuery = new GetEventDetailQuery() {Id = id};
            return Ok(await _mediator.Send(getEventDetailQuery));
        }

        [HttpPost(Name = "AddEvent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin,DTH")]
        public async Task<ActionResult<Guid>> AddEvent([FromBody] CreateEventCommand createEventCommand)
        {
            var id = await _mediator.Send(createEventCommand);
            return id;
        }

        [HttpPut(Name = "UpdateEvent")]
        [ProducesResponseType(statusCode:StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode:StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin,DTH")]
        public async Task<ActionResult> UpdateEvent([FromBody] UpdateEventCommand updateEventCommand)
        {
            await _mediator.Send(updateEventCommand);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteEvent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin,DTH")]
        public async Task<ActionResult> DeleteEvent(Guid id)
        {
            await _mediator.Send(new DeleteEventCommand() {EventId = id});
            return NoContent();
        }

        [HttpGet("GetEventsBy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetEventListByResponse>> GetEventsListBy([FromQuery] GetEventsListByQuery request)
        {
            var dtos = await _mediator.Send(request);
            return Ok(dtos);
        }


        private GetEventsListResponse GeneratePageList(GetEventsListQuery queryParameters, GetEventsListResponse response)
        {
            if (response.CurrentPage > 1)
            {
                var prevRoute = Url.RouteUrl(nameof(GetAllEvents), new { limit = queryParameters.Limit, page = queryParameters.Page - 1 });

                response.AddResourceLink(LinkedResourceType.Prev, prevRoute);
            }
            if (response.CurrentPage < response.TotalPages)
            {
                var nextRoute = Url.RouteUrl(nameof(GetAllEvents), new { limit = queryParameters.Limit, page = queryParameters.Page + 1 });

                response.AddResourceLink(LinkedResourceType.Next, nextRoute);
            }

            return response;
        }

        [HttpGet("GetEventsWithContent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<EventListWithContentVm>>> GetEventsListWithContentAsync(
            [FromQuery] GetEventsListWithContentQuery request)
        {
            var dtos = await _mediator.Send(request);
            return Ok(dtos);
        }

        /// <summary>
        /// Canh bao thien tai
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("disasterWarning", Name = nameof(GetDisasterWarningAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<GetEventsListResponse>> GetDisasterWarningAsync([FromBody] GetEventsListQuery query)
        {
            if (!query.CategoryId.HasValue || query.CategoryId.Equals(Guid.Empty))
                query.CategoryId = Guid.Parse("e78c78b7-80d1-4f3b-3014-08d91e5e4dfa");
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }

        /// <summary>
        /// Thong tin khuyen cao
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("recommendedInformation", Name = nameof(GetRecommendedInformationAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<GetEventsListResponse>> GetRecommendedInformationAsync([FromBody] GetEventsListQuery query)
        {
            if (!query.CategoryId.HasValue || query.CategoryId.Equals(Guid.Empty))
                query.CategoryId = Guid.Parse("580FFB36-2C72-4642-CB46-08D91FA2C701");
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }

        /// <summary>
        /// Chuyen muc KT-VH-XH
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("socioculturalEngineering", Name = nameof(GetSocioculturalEngineeringAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<GetEventsListResponse>> GetSocioculturalEngineeringAsync([FromBody] GetEventsListQuery query)
        {
            if (!query.CategoryId.HasValue || query.CategoryId.Equals(Guid.Empty))
                query.CategoryId = Guid.Parse("eededf06-2e83-458d-9e0e-08d92ce117ec");
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }

        /// <summary>
        /// Thoi tiet du lich
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("travelWeather", Name = nameof(GetTravelWeatherAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<GetEventsListResponse>> GetTravelWeatherAsync([FromBody] GetEventsListQuery query)
        {
            if (!query.CategoryId.HasValue || query.CategoryId.Equals(Guid.Empty))
                query.CategoryId = Guid.Parse("d34d4116-51f8-4539-5d1d-08d942e67599");
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }

        /// <summary>
        /// Thoi tiet nong vu
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("agriculturalWeather", Name = nameof(GetAgriculturalWeatherAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<GetEventsListResponse>> GetAgriculturalWeatherAsync([FromBody] GetEventsListQuery query)
        {
            if (!query.CategoryId.HasValue || query.CategoryId.Equals(Guid.Empty))
                query.CategoryId = Guid.Parse("2815e0a9-d15f-4d16-5d1e-08d942e67599");
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }

        /// <summary>
        /// Thoi tiet giao thong
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("trafficWeather", Name = nameof(GetTrafficWeatherAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<GetEventsListResponse>> GetTrafficWeatherAsync([FromBody] GetEventsListQuery query)
        {
            if (!query.CategoryId.HasValue || query.CategoryId.Equals(Guid.Empty))
                query.CategoryId = Guid.Parse("fdb895d3-a2e3-49f3-5d1f-08d942e67599");
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }

        /// <summary>
        /// Thoi tiet nguy hiem
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("dangerousWeather", Name = nameof(GetDangerousWeatherAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<GetEventsListResponse>> GetDangerousWeatherAsync([FromBody] GetEventsListQuery query)
        {
            if (!query.CategoryId.HasValue || query.CategoryId.Equals(Guid.Empty))
                query.CategoryId = Guid.Parse("031d1a69-900e-4b63-5d20-08d942e67599");
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }

        /// <summary>
        /// Thuy van
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("hydrological", Name = nameof(GetHydrologicalAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<GetEventsListResponse>> GetHydrologicalAsync([FromBody] GetEventsListQuery query)
        {
            if (!query.CategoryId.HasValue || query.CategoryId.Equals(Guid.Empty))
                query.CategoryId = Guid.Parse("92fb2fa2-12e1-4871-2ecf-08d94344e5e0");
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }

        /// <summary>
        /// Cac trang thai thoi tiet
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("weatherStates", Name = nameof(GetWeatherStatesAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<GetEventsListResponse>> GetWeatherStatesAsync([FromBody] GetEventsListQuery query)
        {
            if (!query.CategoryId.HasValue || query.CategoryId.Equals(Guid.Empty))
                query.CategoryId = Guid.Parse("a54d6936-8789-42e4-6515-08d944a940ef");
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }

        /// <summary>
        /// Dieu hanh san xuat
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("executiveProducer", Name = nameof(GetExecutiveProducerAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin,DTH")]
        public async Task<ActionResult<GetEventsListResponse>> GetExecutiveProducerAsync([FromBody] GetEventsListQuery query)
        {
            if (!query.CategoryId.HasValue || query.CategoryId.Equals(Guid.Empty))
                query.CategoryId = Guid.Parse("4c14b202-72bf-4209-ffd1-08d9486010c7");
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }

        /// <summary>
        /// Phong chong thien tai
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("disasterPrevention", Name = nameof(GetDisasterPreventionAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,Admin,DTH")]
        public async Task<ActionResult<GetEventsListResponse>> GetDisasterPreventionAsync([FromBody] GetEventsListQuery query)
        {
            if (!query.CategoryId.HasValue || query.CategoryId.Equals(Guid.Empty))
                query.CategoryId = Guid.Parse("e95d0a6f-73a4-4cb8-ffd2-08d9486010c7");
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }
        
        [HttpPost("get-all-events-mobile", Name = nameof(GetAllEventsForMobile))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetEventsListResponse>> GetAllEventsForMobile([FromBody] GetEventsListQuery query)
        {
            var dtos = await _mediator.Send(query);
            return Ok(GeneratePageList(query, dtos));
        }

        [HttpPost("get-most-view", Name = nameof(GetMostViewEventAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<EventMostViewResponse>> GetMostViewEventAsync([FromBody] EventMostViewQuery query)
        {
            var dtos = await _mediator.Send(query);
            return Ok(dtos);
        }

    }
}