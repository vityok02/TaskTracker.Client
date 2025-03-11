using Domain.Models.Identity;
using FluentValidation;
using Services.Extensions;

namespace Client.Validators.Identity;

public sealed class LoginModelValidator
    : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Email)
            .ApplyEmailRules();

        RuleFor(x => x.Password)
            .ApplyPasswordRules();
    }
}
