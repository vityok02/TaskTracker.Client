using Domain.Models.Identity;
using FluentValidation;
using Services.Extensions;

namespace Client.Validators.Identity;

public class SetPasswordModelValidator
    : AbstractValidator<SetPasswordModel>
{
    public SetPasswordModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Password)
            .ApplyPasswordRules();

        RuleFor(x => x.ConfirmedPassword)
        .ApplyConfirmedPasswordRules(x => x.Password);
    }
}
