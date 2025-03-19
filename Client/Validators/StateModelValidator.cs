using Domain.Models;
using FluentValidation;

namespace Client.Validators;

public class StateModelValidator
    : AbstractValidator<StateModel>
{
    public StateModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");
    }
}
