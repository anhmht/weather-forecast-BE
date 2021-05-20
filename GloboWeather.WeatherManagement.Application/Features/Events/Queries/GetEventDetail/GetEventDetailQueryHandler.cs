using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventDetail
{
    public class GetEventDetailQueryHandler : IRequestHandler<GetEventDetailQuery, EventDetailVm>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Event> _eventRepository;
        private readonly IAsyncRepository<Category> _categoryRepository;

        public GetEventDetailQueryHandler(IMapper mapper, IAsyncRepository<Event> eventRepository, IAsyncRepository<Category> categoryRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<EventDetailVm> Handle(GetEventDetailQuery request, CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetByIdAsync(request.Id);
            var eventDetailDto = _mapper.Map<EventDetailVm>(@event);

            var catogory = await _categoryRepository.GetByIdAsync(@event.CategoryId);
            if (catogory == null)
            {
                throw new NotFoundException(nameof(Event), request.Id);
            }
            eventDetailDto.Category = _mapper.Map<CategoryDto>(catogory);
            
            return eventDetailDto;
        }
    }
}