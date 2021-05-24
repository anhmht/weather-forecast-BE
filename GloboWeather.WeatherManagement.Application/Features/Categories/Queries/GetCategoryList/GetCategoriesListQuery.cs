using System.Collections.Generic;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Categories.Queries.GetCategoryList
{
    public class GetCategoriesListQuery : IRequest<List<CategoriesListVm>>
    {
        
    }
}