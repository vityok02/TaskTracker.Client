using Domain.Models.Identity;
using FluentValidation;
using Services.Extensions;

namespace Client.Validators.Identity;

public sealed class ChangePasswordModelValidator
    : AbstractValidator<ChangePasswordModel>
{
    public ChangePasswordModelValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .ApplyPasswordRules();

        RuleFor(x => x.NewPassword)
            .ApplyPasswordRules();

        RuleFor(x => x.ConfirmedPassword)
            .ApplyConfirmedPasswordRules(x => x.NewPassword);
    }
}
