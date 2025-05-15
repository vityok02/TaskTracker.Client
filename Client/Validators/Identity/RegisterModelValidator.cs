using Domain.Models.Identity;
using FluentValidation;
using Services.Extensions;

namespace Client.Validators.Identity;

internal sealed class RegisterModelValidator
    : AbstractValidator<RegisterModel>
{
    public RegisterModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Username)
            .NotEmpty()
                .WithMessage("The field must not be empty ")
            .MinimumLength(3)
                .WithMessage("The username must be at least 5 charaters long")
            .MaximumLength(20)
                .WithMessage("The username must be less then 20 characters long");

        RuleFor(x => x.Email)
            .ApplyEmailRules();

        RuleFor(x => x.Password)
            .ApplyPasswordRules();

        RuleFor(x => x.ConfirmedPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords must match");

        RuleFor(x => x.ConfirmedPassword)
            .ApplyConfirmedPasswordRules(x => x.Password);
    }
}
