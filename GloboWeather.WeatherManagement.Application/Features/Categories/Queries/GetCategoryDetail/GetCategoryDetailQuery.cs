using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Categories.Queries.GetCategoryDetail
{
    public class GetCategoryDetailQuery : IRequest<CategoryDetailVm>
    {
        public Guid CategoryId { get; set; }
    }
}