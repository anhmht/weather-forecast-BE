using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Comments.Commands.CreateComment;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.ChangeStatus;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.CreatePost;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> CreatePostAsync([FromBody] CreatePostCommand createPostCommand)
        {
            var response = await _mediator.Send(createPostCommand);
            return response;
        }

        [HttpPost("create-comment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> CreateCommentAsync([FromBody] CreateCommentCommand createCommentCommand)
        {
            var response = await _mediator.Send(createCommentCommand);
            return response;
        }

        [HttpPut("approve-post/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> ApprovePostAsync([FromRoute] Guid id)
        {
            var request = new ChangeStatusCommand
            {
                Id = id,
                PostStatusId = (int) PostStatus.Public,
                IsApproval = true,
                IsChangePostStatus = true
            };
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpPut("approve-comment/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> ApproveCommentAsync([FromRoute] Guid id)
        {
            var request = new ChangeStatusCommand
            {
                Id = id,
                PostStatusId = (int)PostStatus.Public,
                IsApproval = true,
                IsChangePostStatus = false
            };
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpPut("change-post-status/{id}&statusId={statusId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> ChangePostStatusAsync([FromRoute] Guid id, int statusId)
        {
            var request = new ChangeStatusCommand
            {
                Id = id,
                PostStatusId = statusId,
                IsApproval = false,
                IsChangePostStatus = true
            };
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpPut("change-comment-status/{id}&statusId={statusId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> ChangeCommentStatusAsync([FromRoute] Guid id, int statusId)
        {
            var request = new ChangeStatusCommand
            {
                Id = id,
                PostStatusId = statusId,
                IsApproval = false,
                IsChangePostStatus = false
            };
            await _mediator.Send(request);
            return NoContent();
        }
    }
}