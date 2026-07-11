using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
            if (await userManager.FindByEmailAsync(request.Email) != null)
            {
                return Results.BadRequest(ApiErrors.UserIsAlreadyExists);
            }
            
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
            
            if (context.Request.Cookies.TryGetValue(CookieKeys.AuthTokenKey, out string? _))
            {
                return Results.BadRequest(ApiErrors.UserIsAlreadyLoggedIn);
            }
            
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
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(jwtOptions.ExpiresHours)
            };
            
            context.Response.Cookies.Append(CookieKeys.AuthTokenKey, token, cookieOptions);
            return Results.Ok();
        }).WithName(ApiEndpointNames.LoginUser);

        group.MapPost("/logout", (HttpContext context) =>
        {
            context.Response.Cookies.Delete(CookieKeys.AuthTokenKey, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            return Results.NoContent();
        }).WithName(ApiEndpointNames.LogoutUser);

        group.MapGet("/status",  (HttpContext context, IOptions<JwtOptions> options) =>
        {
            var hasCookie = context.Request.Cookies.TryGetValue(CookieKeys.AuthTokenKey, out string? cookie);
            if (!hasCookie || cookie == null)                 return Results.Json(new { error = "SUKA", detail = "POLNAYA PIZDA" }, statusCode: 401);
            ;
            var jwtHandler = new JwtSecurityTokenHandler();
            try
            {
                jwtHandler.ValidateToken(cookie, new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey))
                }, out _);
            }
            catch (Exception e)
            {
                return Results.Json(new { error = e.Message, detail = e.InnerException?.Message }, statusCode: 401);
            }

            return Results.Ok();
        }).WithName(ApiEndpointNames.GetUserStatus);
    }
}