using FluentValidation;

namespace ToDoList.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class 
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argumentToValidate = context.Arguments.OfType<T>().FirstOrDefault();
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

        if (argumentToValidate != null && validator != null)
        {
            var validationResult = await validator.ValidateAsync(argumentToValidate);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }
        }
        return await next(context);
    }
}