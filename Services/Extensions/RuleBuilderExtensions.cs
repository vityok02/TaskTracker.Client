using FluentValidation;

namespace Services.Extensions;

public static class RuleBuilderExtensions
{
    public static IRuleBuilder<T, string> ApplyPasswordRules<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithMessage("Please enter a password")
            .MinimumLength(8)
                .WithMessage("Password cannot contain less than 8 symbols")
            .MaximumLength(50)
                .WithMessage("Password cannot contain more than 50 symbols")
            .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
                .WithMessage("Password must contain at least one digit.");
    }

    public static IRuleBuilder<T, string> ApplyEmailRules<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithMessage("Please enter an your email address")
            .EmailAddress()
                .WithMessage("Please enter a valid email address");
    }
}