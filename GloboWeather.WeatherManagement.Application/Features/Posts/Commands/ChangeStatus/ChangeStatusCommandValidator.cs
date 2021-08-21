using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace GloboWeather.WeatherManagement.Application.Features.Posts.Commands.ChangeStatus
{
    public class ChangeStatusCommandValidator : AbstractValidator<ChangeStatusCommand>
    {
        public ChangeStatusCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.PostStatusId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
