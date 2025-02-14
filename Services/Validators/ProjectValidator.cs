using Domain.Models;
using FluentValidation;
using Services.Extensions;

namespace Services.Validators;

internal sealed class ProjectValidator
    : AbstractValidator<ProjectModel>
{
    public ProjectValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .ApplyNameRules();

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description ?? "")
                .ApplyDescriptionRules();
        });
    }
}
