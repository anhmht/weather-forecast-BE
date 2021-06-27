
using AutoMapper;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using Moq;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Features.Categories.Commands.CreateCategory;
using GloboWeather.WeatherManagement.Application.Profiles;
using GloboWeather.WeatherManagement.Application.UnitTests.Mocks;
using Xunit;

namespace GloboWeather.WeatherManagement.Application.UnitTests.Categories.Commands
{
    public class CreateCategoryTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAsyncRepository<Category>> _mockCategoryRepository;
        private readonly Mock<IUnitOfWork> _unitofWork;
        public CreateCategoryTests()
        {
           // _unitofWork.
            _mockCategoryRepository = RepositoryMocks.GetCategoryRepository();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidCategory_AddedToCategoriesRepo()
        {
           // var handler = new CreateCategoryCommandHandler(_mapper, _mockCategoryRepository.Object);
        }
    }
}