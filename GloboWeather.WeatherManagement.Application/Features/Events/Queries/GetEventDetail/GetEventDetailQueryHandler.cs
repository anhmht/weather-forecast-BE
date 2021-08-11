using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventDetail
{
    public class GetEventDetailQueryHandler : IRequestHandler<GetEventDetailQuery, EventDetailVm>
    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IEventDocumentRepository _eventDocumentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetEventDetailQueryHandler(IMapper mapper,
            IEventRepository eventRepository,
            ICategoryRepository categoryRepository,
            IStatusRepository statusRepository,
            IEventDocumentRepository eventDocumentRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _categoryRepository = categoryRepository;
            _statusRepository = statusRepository;
            _eventDocumentRepository = eventDocumentRepository;
            _unitOfWork = unitOfWork;
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

            var stautus = await _statusRepository.GetByIdAsync(@event.StatusId);

            if (stautus == null)
            {
                throw new NotFoundException(nameof(Event), request.Id);
            }

            eventDetailDto.Status = _mapper.Map<StatusDto>(stautus);

            eventDetailDto.Documents = await _eventDocumentRepository.GetByEventIdAsync(@event.EventId);

            #region Count Event view
            try
            {
                await _unitOfWork.EventViewCountRepository.AddEventViewCountAsync(eventDetailDto.EventId);
            }
            catch
            {
                //Ignore
            }
            #endregion

            return eventDetailDto;
        }
    }
}