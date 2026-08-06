using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Infrastructure.Authentication;
using ToDoList.Infrastructure.Data;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.Shared.Constants;

namespace ToDoList.Shared.Extensions;

public static class AuthenticationExtensions
{
    private static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()
                         ?? throw new InvalidOperationException(ApiErrors.JwtOptionsSectionMissing);

        if (string.IsNullOrWhiteSpace(jwtOptions.SecretKey))
        {
            throw new InvalidOperationException(ApiErrors.JwtSecretKeyNotConfigured);
        }

        services.Configure<JwtOptions>(options =>
        {
            options.SecretKey = jwtOptions.SecretKey;
            options.ExpiresHours = jwtOptions.ExpiresHours;
        });
        
        return services;
    }
    
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;

                options.User.RequireUniqueEmail = false; 
            })
            .AddEntityFrameworkStores<AppDbContext>();
        
        services.ConfigureJwt(configuration);
        
        services.AddScoped<IJwtProvider, JwtProvider>();
        
        var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["TodoAccessToken"];
                        
                            return Task.CompletedTask;
                        }
                    };
                }
            );
        services.AddAuthorization();
        return services;
    }
}