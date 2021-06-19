using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Exceptions;
using GloboWeather.WeatherManagement.Domain.Entities;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryToUpdate = await _unitOfWork.CategoryRepository.GetByIdAsync(request.CategoryId);
            
            if (categoryToUpdate == null)
            {
                throw new NotFoundException(nameof(Event), request.CategoryId);
            }

            var validator = new UpdateCategoryCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Count > 0)
            {
                throw new ValidationException(validationResult);
            }

            _mapper.Map(request, categoryToUpdate, typeof(UpdateCategoryCommand), typeof(Category));

            _unitOfWork.CategoryRepository.Update(categoryToUpdate);
            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
    }
}