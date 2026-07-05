using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ToDoList.Dtos;
using ToDoList.Infrastructure.Authentication;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.Shared.Constants;

namespace ToDoList.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", async (RegisterUserRequest request, UserManager<ApplicationUser> userManager) =>
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };
            
            
            var registrationResult = await userManager.CreateAsync(user, request.Password);
            return !registrationResult.Succeeded ? Results.BadRequest(registrationResult.Errors) : Results.Ok(new { user.Id, user.Email });
        }).WithName(ApiEndpointNames.RegisterUser);
        
        group.MapPost("/login", async (
            LoginUserRequest request, 
            UserManager<ApplicationUser> userManager, 
            IJwtProvider provider,
            HttpContext context, 
            IOptions<JwtOptions> options) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            var jwtOptions = options.Value;
            if (user == null)
            {
                return Results.BadRequest(ApiErrors.IncorrectEmailOrPassword);
            }

            var passwordCheck = await userManager.CheckPasswordAsync(user, request.Password);
            
            if (!passwordCheck)
            {
                return Results.BadRequest(ApiErrors.IncorrectEmailOrPassword);
            }

            var token = provider.GenerateToken(user);
            
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(jwtOptions.ExpiresHours)
            };
            
            context.Response.Cookies.Append(CookieKeys.AuthTokenKey, token, cookieOptions);
            return Results.Ok();
        }).WithName(ApiEndpointNames.LoginUser);
    }
}