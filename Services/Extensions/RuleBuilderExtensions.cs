using FluentValidation;
using System.Linq.Expressions;

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

    public static IRuleBuilder<T, string> ApplyConfirmedPasswordRules<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        Expression<Func<T, string>> toCompare)
    {
        return ruleBuilder
            .Equal(toCompare)
                .WithMessage("Passwords do not match.");

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

    public static IRuleBuilder<T, string> ApplyNameRules<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        int minimumLength = 3;
        int maximumLength = 50;

        return ruleBuilder
            .NotEmpty()
                .WithMessage("Please enter a value")
            .MinimumLength(minimumLength)
                .WithMessage($"The field must contain at least {minimumLength} symbols")
            .MaximumLength(maximumLength)
                .WithMessage($"The field must contain no more than {maximumLength} symbols");
    }

    public static IRuleBuilder<T, string> ApplyDescriptionRules<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        int minimumLength = 3;
        int maximumLength = 500;

        return ruleBuilder
            .MinimumLength(minimumLength)
                .WithMessage($"The field must contain at least {minimumLength} symbols")
            .MaximumLength(maximumLength)
                .WithMessage($"The field must contain no more than {maximumLength} symbols");
    }
}