using FluentValidation;
using ToDoList.Dtos;

namespace ToDoList.Validators;

public class TagRequestValidator : AbstractValidator<TagRequest>
{
    public TagRequestValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage("Название тега не должно быть пустым")
            .MaximumLength(20).WithMessage("Длина названия тега не должна превышать 20 символов");
        RuleFor(request => request.Color)
            .NotEmpty().WithMessage("Цвет тега обязателен")
            .Matches("^#(?:[0-9a-fA-F]{3}){1,2}$|^#(?:[0-9a-fA-F]{4}){1,2}$")
            .WithMessage("Цвет должен быть валидным HEX-кодом (например, #FF5733)");
    }
}