using ToDoList.Dtos;
using ToDoList.Infrastructure.Data;
using ToDoList.Services;
using ToDoList.Shared.Constants;
using ToDoList.Shared.Extensions;
using ToDoList.Shared.Mappings;

namespace ToDoList.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        var todosGroup = app.MapGroup("/api/todos").RequireAuthorization().RequireUserContext();

        todosGroup.MapPost("/", async (
            TodoRequest todoRequest,
            TodoService todoService,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var todo = await todoService.Create(todoRequest.ToDetails(context.GetUserId()), todoRequest.TagIds, cancellationToken);
            return Results.CreatedAtRoute(ApiEndpointNames.GetTodo, new {id = todo.Id} , todo.ToResponse());
        })
        .Validate<TodoRequest>()
        .WithName(ApiEndpointNames.CreateTodo);

        todosGroup.MapGet("", async (
                string? folder,
                string? dateTime,
                [AsParameters] GetTodosQuery getTodosQuery,
                TodoService todoService,
                HttpContext context,
                CancellationToken cancellationToken) =>
        {
            var todos = await todoService.GetAll(context.GetUserId(), folder ?? string.Empty, dateTime, getTodosQuery, cancellationToken);
            return Results.Ok(todos);
        })
        .WithName(ApiEndpointNames.GetAllTodos);

        var concreteTodoGroup = todosGroup.MapGroup("/{id:guid}").AddEndpointFilter(async (context, next) =>
        {
            var userId = context.HttpContext.GetUserId();
            var todoId = context.Arguments.OfType<Guid>().FirstOrDefault();
            var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

            var todo = await db.Todos.FindAsync(new object?[] { todoId }, context.HttpContext.RequestAborted);
            if (todo == null || todo.UserId != userId)
            {
                return Results.NotFound();
            }

            return await next(context);
        });

        concreteTodoGroup.MapGet("", async (Guid id, TodoService todoService, CancellationToken cancellationToken) =>
        {
            var todo = await todoService.Get(id, cancellationToken);

            return todo != null ? Results.Ok(todo) : Results.NotFound();
        })
        .WithName(ApiEndpointNames.GetTodo);

        concreteTodoGroup.MapDelete("", async (Guid id, TodoService todoService, CancellationToken cancellationToken) =>
        {
            var result = await todoService.Remove(id, cancellationToken);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName(ApiEndpointNames.DeleteTodo);

        concreteTodoGroup.MapPut("", async (
            Guid id,
            TodoRequest todoRequest,
            TodoService todoService,
            CancellationToken cancellationToken) =>
        {
            var todoDetails = todoRequest.ToDetails();
            var updatedTodo = await todoService.Update(id, todoDetails, cancellationToken);
            return updatedTodo != null ? Results.Ok(updatedTodo) : Results.NotFound();
        })
        .Validate<TodoRequest>()
        .WithName(ApiEndpointNames.EditTodo);

        concreteTodoGroup.MapPut("/complete", async (Guid id, TodoService todoService, CancellationToken cancellationToken) =>
        {
            var result = await todoService.Complete(id, cancellationToken);
            return result ? Results.Ok() : Results.NotFound();
        })
        .WithName(ApiEndpointNames.CompleteTodo);

        concreteTodoGroup.MapPut("/uncomplete", async (Guid id, TodoService todoService, CancellationToken cancellationToken) =>
        {
            var result = await todoService.Uncomplete(id, cancellationToken);
            return result ? Results.Ok() : Results.NotFound();
        })
        .WithName(ApiEndpointNames.UncompleteTodo);
    }
}
