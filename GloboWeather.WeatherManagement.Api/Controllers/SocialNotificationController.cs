using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Commands.UpdateIsReadSocialNotification;
using GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Queries.GetListSocialNotification;
using GloboWeather.WeatherManagement.Application.Features.SocialNotifications.Queries.GetUnReadCountSocialNotification;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialNotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SocialNotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("get-list", Name = nameof(GetListSocialNotificationAsync))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetListSocialNotificationResponse>> GetListSocialNotificationAsync([FromBody] GetListSocialNotificationQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("set-read-notification/{id}", Name = nameof(UpdateIsReadSocialNotificationAsync))]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateIsReadSocialNotificationAsync([FromRoute] Guid id)
        {
            var request = new UpdateIsReadSocialNotificationCommand { Id = id };
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpGet("get-count-unread-notification", Name = "GetCountUnreadNotificationAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetUnreadCountSocialNotificationResponse>> GetUnreadCountNotificationAsync()
        {
            var request = new GetUnreadCountSocialNotificationQuery();
            return Ok(await _mediator.Send(request));
        }

    }
}
