using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Comments.Commands.CreateComment;
using GloboWeather.WeatherManagement.Application.Features.Comments.Commands.UpdateComment;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.AddActionIcon;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.ChangeStatus;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.CreatePost;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.RemoveActionIcon;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.SharePost;
using GloboWeather.WeatherManagement.Application.Features.Posts.Commands.UpdatePost;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentList;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentListOfUser;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostDetail;
using GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostList;
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

        [HttpPost(Name = "CreatePost")]
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
                PostStatusId = (int)PostStatus.Public,
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

        [HttpPut(Name = "UpdatePost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdatePostAsync([FromBody] UpdatePostCommand createPostCommand)
        {
            await _mediator.Send(createPostCommand);
            return NoContent();
        }

        [HttpPut("update-comment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateCommentAsync([FromBody] UpdateCommentCommand createPostCommand)
        {
            await _mediator.Send(createPostCommand);
            return NoContent();
        }

        [HttpPost("add-post-action-icon")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> AddPostActionIconAsync([FromBody] AddActionIconCommand addActionIconCommand)
        {
            addActionIconCommand.IsPost = true;
            await _mediator.Send(addActionIconCommand);
            return NoContent();
        }

        [HttpPost("add-comment-action-icon")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> AddCommentActionIconAsync([FromBody] AddActionIconCommand addActionIconCommand)
        {
            addActionIconCommand.IsPost = false;
            await _mediator.Send(addActionIconCommand);
            return NoContent();
        }

        [HttpDelete("remove-post-action-icon/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> RemovePostActionIconAsync([FromRoute] Guid id)
        {
            var request = new RemoveActionIconCommand()
            {
                Id = id,
                IsPost = true
            };
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpDelete("remove-comment-action-icon/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> RemoveCommentActionIconAsync([FromRoute] Guid id)
        {
            var request = new RemoveActionIconCommand()
            {
                Id = id,
                IsPost = false
            };
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpPost("share")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> SharePostAsync([FromBody] SharePostCommand request)
        {
            var response = await _mediator.Send(request);
            return response;
        }

        [HttpPost("get-list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetPostListResponse>> GetPostListAsync([FromBody] GetPostListQuery request)
        {
            request.IsUserTimeLine = false;
            var response = await _mediator.Send(request);
            return response;
        }

        [HttpPost("get-list-user-time-line")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetPostListResponse>> GetPostListOnUserTimeLineAsync([FromBody] GetPostListQuery request)
        {
            request.IsUserTimeLine = true;
            var response = await _mediator.Send(request);
            return response;
        }

        [HttpGet("{id}", Name = "GetPostDetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetPostDetailResponse>> GetPostDetailAsync([FromRoute] Guid id)
        {
            var request = new GetPostDetailQuery { Id = id };
            var response = await _mediator.Send(request);
            return response;
        }

        [HttpDelete("{id}", Name = "DeletePost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeletePostAsync([FromRoute] Guid id)
        {
            var request = new ChangeStatusCommand
            {
                Id = id,
                PostStatusId = (int)PostStatus.Deleted,
                IsApproval = true,
                IsChangePostStatus = true
            };
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpDelete("comment/{id}", Name = "DeleteComment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteCommentAsync([FromRoute] Guid id)
        {
            var request = new ChangeStatusCommand
            {
                Id = id,
                PostStatusId = (int)PostStatus.Deleted,
                IsApproval = true,
                IsChangePostStatus = false
            };
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpPost("get-list-comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetCommentListResponse>> GetListCommentAsync([FromBody] GetCommentListQuery request)
        {
            var response = await _mediator.Send(request);
            return response;
        }

        [HttpPost("get-list-comment-of-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetCommentListOfUserResponse>> GetListCommentOfUserAsync([FromBody] GetCommentListOfUserQuery request)
        {
            var response = await _mediator.Send(request);
            return response;
        }

    }
}