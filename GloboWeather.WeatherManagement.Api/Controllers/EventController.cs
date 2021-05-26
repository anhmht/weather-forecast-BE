using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Events.Commands.CreateEvent;
using GloboWeather.WeatherManagement.Application.Features.Events.Commands.DeleteEvent;
using GloboWeather.WeatherManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsList;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListByCateIdAndStaId;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using GloboWeather.WeatherManagement.Application.Helpers.Paging;
using MediatR;
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

        [HttpGet(Name = nameof(GetAllEvents))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetEventsListResponse>> GetAllEvents([FromQuery] GetEventsListQuery query)
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
        public async Task<ActionResult<Guid>> AddEvent([FromBody] CreateEventCommand createEventCommand)
        {
            var id = await _mediator.Send(createEventCommand);
            return id;
        }

        [HttpPut(Name = "UpdateEvent")]
        [ProducesResponseType(statusCode:StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode:StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateEvent([FromBody] UpdateEventCommand updateEventCommand)
        {
            await _mediator.Send(updateEventCommand);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteEvent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteEvent(Guid id)
        {
            await _mediator.Send(new DeleteEventCommand() {EventId = id});
            return NoContent();
        }

        [HttpGet("GetEventsBy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<EventListCateStatusVm>>> GetEventsListBy([FromQuery] GetEventsListByQuery request)
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
        
        
    }
}