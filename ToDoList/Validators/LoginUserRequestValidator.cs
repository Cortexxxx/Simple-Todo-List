using FluentValidation;
using ToDoList.Dtos;
using ToDoList.Shared.Constants;

namespace ToDoList.Validators;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(request => request.Email).NotEmpty().WithMessage(ApiErrors.EmailRequired);
        RuleFor(request => request.Password).NotEmpty().WithMessage(ApiErrors.PasswordRequired);
    }
}