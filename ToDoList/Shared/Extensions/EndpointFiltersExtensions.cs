using System.Security.Claims;

namespace ToDoList.Shared.Extensions;

public static class EndpointFiltersExtensions
{
    public static RouteGroupBuilder RequireUserContext(this RouteGroupBuilder builder)
    {
        return builder.AddEndpointFilter(async (context, next) =>
        {
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userGuid))
            {
                return Results.Unauthorized();
            }
            
            context.HttpContext.SetUserId(userGuid);
            
            return await next(context);
        });
    } 
}