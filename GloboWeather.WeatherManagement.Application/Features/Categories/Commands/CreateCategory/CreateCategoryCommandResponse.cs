using GloboWeather.WeatherManegement.Application.Responses;

namespace GloboWeather.WeatherManagement.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandResponse : BaseResponse
    {
        public CreateCategoryCommandResponse() : base()
        {
            
        }
        
        public CreateCategoryDto Category { get; set; }
    }
}