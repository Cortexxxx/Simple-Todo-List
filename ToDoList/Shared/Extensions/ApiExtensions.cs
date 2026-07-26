using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Infrastructure.Authentication;

namespace ToDoList.Shared.Extensions;

public static class ApiExtensions
{

    
    public static void SetUserId(this HttpContext context, Guid userId)
    {
        context.Items["UserId"] = userId;
    }

    public static Guid GetUserId(this HttpContext context) => 
        context.Items["UserId"] is Guid userId ? userId : Guid.Empty;


}