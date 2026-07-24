using FluentValidation;
using ToDoList.Dtos;
using ToDoList.Shared.Constants;

namespace ToDoList.Validators;

public class TodoRequestValidator : AbstractValidator<TodoRequest>
{
    public TodoRequestValidator()
    {
        RuleFor(request => request.Title).NotEmpty().WithMessage(ApiErrors.TodoTitleRequired)
            .MaximumLength(100).WithMessage(ApiErrors.TodoTitleMaxLength);

        RuleFor(request => request.Description).MaximumLength(2000).WithMessage(ApiErrors.TodoDescriptionMaxLength);
    }
}