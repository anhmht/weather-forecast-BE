using System;
using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateActionOrder
{
    class UpdateActionOrderValidator : AbstractValidator<UpdateActionOrderCommand>
    {
        public UpdateActionOrderValidator()
        {
            RuleForEach(x => x.ActionOrders)
                .Where(x => !x.ActionId.Equals(Guid.Empty))
                .NotNull().WithMessage("{PropertyName} is required.");
        }
    }
}
