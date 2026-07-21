using FluentValidation;
using ToDoList.Dtos;

namespace ToDoList.Validators;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(request => request.Email).NotEmpty().WithMessage("Почта не должна быть пустой");
        RuleFor(request => request.Password).NotEmpty().WithMessage("Пароль не должен быть пустой");
    }
}