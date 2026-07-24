using FluentValidation;
using ToDoList.Dtos;
using ToDoList.Shared.Constants;

namespace ToDoList.Validators;

public class TagRequestValidator : AbstractValidator<TagRequest>
{
    public TagRequestValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ApiErrors.TagNameRequired)
            .MaximumLength(20).WithMessage(ApiErrors.TagNameMaxLength);
        RuleFor(request => request.Color)
            .NotEmpty().WithMessage(ApiErrors.TagColorRequired)
            .Matches("^#(?:[0-9a-fA-F]{3}){1,2}$|^#(?:[0-9a-fA-F]{4}){1,2}$")
            .WithMessage(ApiErrors.TagColorInvalidHex);
    }
}