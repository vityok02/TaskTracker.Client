using Domain.Models;
using FluentValidation;

namespace Client.Validators;

public class CommentModelValidator
    : AbstractValidator<CommentModel>
{
    public CommentModelValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("Please, enter comment");
    }
}