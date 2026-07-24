using FluentValidation;
using Microsoft.AspNetCore.Identity;
using ToDoList.Dtos;
using ToDoList.Models;
using ToDoList.Shared.Constants;

namespace ToDoList.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator(UserManager<ApplicationUser> userManager)
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage(ApiErrors.EmailRequired)
            .EmailAddress().WithMessage(ApiErrors.EmailInvalid)
            .MustAsync(async (email, _) =>
            {
                var userExists = await userManager.FindByEmailAsync(email);
                return userExists == null;
            })
            .WithMessage(ApiErrors.EmailAlreadyRegistered);

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage(ApiErrors.PasswordRequired)
            .MinimumLength(6).WithMessage(ApiErrors.PasswordMinLength);
    }
}