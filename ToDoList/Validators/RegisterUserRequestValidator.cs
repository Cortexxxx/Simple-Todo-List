using FluentValidation;
using Microsoft.AspNetCore.Identity;
using ToDoList.Dtos;

namespace ToDoList.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator(UserManager<IdentityUser> userManager)
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Почта не должна быть пустой")
            .EmailAddress().WithMessage("Введите корректную почту")
            .MustAsync(async (email, cancellationToken) => 
            {
                var userExists = await userManager.FindByEmailAsync(email);
                return userExists == null;
            })
            .WithMessage("Пользователь с такой почтой уже зарегистрирован");;

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Пароль не должен быть пустой")
            .MinimumLength(6).WithMessage("Минимальная длина пароля 6 символов");
    }
}