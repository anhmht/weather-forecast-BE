using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Api;
using GloboWeather.WeatherManagement.API.IntergrationTests.Base;
using GloboWeather.WeatherManagement.Application.Features.Categories.Queries.GetCategoryList;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace GloboWeather.WeatherManagement.API.IntergrationTests.Controllers
{
    public class CategoryControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CategoryControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsSuccessResult()
        {
            var client = _factory.GetAnonymousClient();

            var response = await client.GetAsync("/api/category/GetAllCategories");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<CategoriesListVm>>(responseString);

            Assert.IsType<List<CategoriesListVm>>(result);
            Assert.NotEmpty(result);

        }
        
        
       
    }
}