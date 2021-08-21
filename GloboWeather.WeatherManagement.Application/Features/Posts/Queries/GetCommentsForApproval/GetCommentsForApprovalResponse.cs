using System;
using GloboWeather.WeatherManagement.Application.Responses;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentsForApproval
{
    public class GetCommentsForApprovalResponse : BasePagingResponse<CommentForApprovalVm>
    {

    }

    public class CommentForApprovalVm
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public Guid? AnonymousUserId { get; set; }
        public string Content { get; set; }
        public string CreateBy { get; set; }
        public string CreatorFullName { get; set; }
        public string CreatorShortName { get; set; }
        public string CreatorAvatarUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public bool HasMedia { get; set; }

    }

}
