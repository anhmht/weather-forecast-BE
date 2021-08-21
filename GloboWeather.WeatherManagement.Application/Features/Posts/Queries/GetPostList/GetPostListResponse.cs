using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetPostList
{
    public class GetPostListResponse
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<PostVm> Posts { get; set; }
    }

    public class PostVm
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string CreateBy { get; set; }
        public string CreatorFullName { get; set; }
        public string CreatorShortName { get; set; }
        public string CreatorAvatarUrl { get; set; }
        public DateTime CreateDate { get; set; }
        [JsonIgnore]
        public string ImageUrls { get; set; }
        [JsonIgnore]
        public string VideoUrls { get; set; }
        public List<string> ListImageUrl { get; set; }
        public List<string> ListVideoUrl { get; set; }
        public int StatusId { get; set; }
        public DateTime? PublicDate { get; set; }
        public string ApprovedByUserName { get; set; }
        public string ApprovedByFullName { get; set; }
        public List<CommentVm> Comments { get; set; }
        public List<ActionIconVm> ActionIcons { get; set; }
        public SharePostVm Shares { get; set; }
        public int NumberOfComment { get; set; }

        public PostVm()
        {
            ListImageUrl = new List<string>();
            ListVideoUrl = new List<string>();
            Comments = new List<CommentVm>();
            ActionIcons = new List<ActionIconVm>();
            Shares = new SharePostVm();
        }
    }

    public class CommentVm
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public string CreateBy { get; set; }
        public string CreatorFullName { get; set; }
        public string CreatorShortName { get; set; }
        public string CreatorAvatarUrl { get; set; }
        public DateTime CreateDate { get; set; }
        [JsonIgnore]
        public string ImageUrls { get; set; }
        [JsonIgnore]
        public string VideoUrls { get; set; }
        public List<string> ListImageUrl { get; set; }
        public List<string> ListVideoUrl { get; set; }
        public int StatusId { get; set; }
        public DateTime? PublicDate { get; set; }
        [JsonIgnore]
        public Guid? AnonymousUserId { get; set; }
        public string ApprovedByUserName { get; set; }
        public string ApprovedByFullName { get; set; }
        public List<ActionIconVm> ActionIcons { get; set; }
        public Guid? ParentCommentId { get; set; }
        public int NumberOfSubComment { get; set; }

        public CommentVm()
        {
            ListImageUrl = new List<string>();
            ListVideoUrl = new List<string>();
            ActionIcons = new List<ActionIconVm>();
        }
    }

    public class ActionIconVm
    {
        public int IconId { get; set; }
        public int Count { get; set; }
        public List<string> FullNames { get; set; }
        public bool IsCurrentUserChecking { get; set; } //Is current login user like/dislike this post/comment

        public ActionIconVm()
        {
            FullNames = new List<string>();
        }
    }

    public class SharePostVm
    {
        public int Count { get; set; }
        public List<string> FullNames { get; set; }

        public SharePostVm()
        {
            FullNames = new List<string>();
        }
    }
}
