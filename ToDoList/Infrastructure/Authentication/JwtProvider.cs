using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Infrastructure.Authentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    
    public string GenerateToken(ApplicationUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Dictionary<string, object>()
        {
            {ClaimTypes.NameIdentifier, user.Id},
            {ClaimTypes.Email, user.Email ?? string.Empty},
        };
        
        return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Claims = claims,
            Expires = DateTime.UtcNow.AddHours(_options.ExpiresHours)
        });
    }
}