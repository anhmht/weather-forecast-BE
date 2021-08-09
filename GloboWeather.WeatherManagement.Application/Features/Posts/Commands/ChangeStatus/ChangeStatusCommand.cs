using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Commands.ChangeStatus
{
    public class ChangeStatusCommand : IRequest
    {
        public Guid Id { get; set; }
        /// <summary>
        /// If FALSE => change comment status
        /// </summary>
        public bool IsChangePostStatus { get; set; }
        public int PostStatusId { get; set; }
        public bool IsApproval { get; set; }
    }
}
