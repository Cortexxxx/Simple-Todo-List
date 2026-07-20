using FluentValidation;
using ToDoList.Dtos;

namespace ToDoList.Validators;

public class TodoRequestValidator : AbstractValidator<TodoRequest>
{
    public TodoRequestValidator()
    {
        RuleFor(request => request.Title).NotEmpty().WithMessage("Название не должно быть пустым")
            .MaximumLength(100).WithMessage("Длина названия не должна превышать 100 символов");
        
        RuleFor(request => request.Description).MaximumLength(2000).WithMessage("Длина названия не должна превышать 2000 символов");
    }
}