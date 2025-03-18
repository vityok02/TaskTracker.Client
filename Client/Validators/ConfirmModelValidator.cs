using Domain.Models;
using FluentValidation;

namespace Client.Validators;

public class ConfirmModelValidator
    : AbstractValidator<ConfirmModel>
{
    public ConfirmModelValidator()
    {
        RuleFor(x => x.Input)
            .NotEmpty()
            .WithMessage("Field could not be empty");
    }
}
