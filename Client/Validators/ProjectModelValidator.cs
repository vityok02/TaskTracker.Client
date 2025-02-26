using Domain.Models;
using FluentValidation;
using Services.Extensions;

namespace Client.Validators;

public sealed class ProjectModelValidator
    : AbstractValidator<ProjectModel>
{
    public ProjectModelValidator()
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
