using Domain.Models;
using FluentValidation;

namespace Client.Validators;

public class ProjectMemberValidator
    : AbstractValidator<ProjectMemberModel>
{
    public ProjectMemberValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty)
            .NotNull();

        RuleFor(x => x.RoleId)
            .NotEqual(Guid.Empty)
            .NotNull();
    }
}