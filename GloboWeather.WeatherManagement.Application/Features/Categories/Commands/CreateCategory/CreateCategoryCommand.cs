using System;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CreateCategoryCommandResponse>
    {
    public string Name { get; set; }
    }
}