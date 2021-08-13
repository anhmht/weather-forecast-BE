using GloboWeather.WeatherManagement.Application.Requests;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Queries.GetCommentListOfUser
{
    public class GetCommentListOfUserQuery : BasePagingRequest, IRequest<GetCommentListOfUserResponse>
    {

    }
}
