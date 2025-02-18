using Domain.Models.Identity;
using FluentValidation;
using Services.Extensions;

namespace Client.Validators.Identity;

public class ResetPasswordModelValidator
    : AbstractValidator<ResetPasswordModel>
{
    public ResetPasswordModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Email)
            .ApplyEmailRules();
    }
}
