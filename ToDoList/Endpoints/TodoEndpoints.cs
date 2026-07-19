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
            HttpContext context) =>
        {
            var todo = await todoService.Create(todoRequest.ToDetails(context.GetUserId()), todoRequest.TagIds);
            return Results.CreatedAtRoute(ApiEndpointNames.GetTodo, new {id = todo.Id} , todo.ToResponse());
        })
        .WithName(ApiEndpointNames.CreateTodo);
        
        todosGroup.MapGet("", async (TodoService todoService, HttpContext context) =>
        {
            var todos = await todoService.GetAll(context.GetUserId(), context.Request.Query["folder"]!, context.Request.Query["dateTime"]);
            return Results.Ok(todos);
        })
        .WithName(ApiEndpointNames.GetAllTodos);


        var concreteTodoGroup = todosGroup.MapGroup("/{id:guid}").AddEndpointFilter(async (context, next) =>
        {
            var userId = context.HttpContext.GetUserId();
            var todoId = context.Arguments.OfType<Guid>().FirstOrDefault();
            var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

            var todo = await db.Todos.FindAsync(todoId);
            if (todo == null || todo.UserId != userId)
            {
                return Results.NotFound();
            }
            
            return await next(context);
        });
        
        concreteTodoGroup.MapGet("", async (Guid id, TodoService todoService) =>
        {
            var todo = await todoService.Get(id);
            return todo != null ? Results.Ok(todo) : Results.NotFound();
        })
        .WithName(ApiEndpointNames.GetTodo);
        
        concreteTodoGroup.MapDelete("", async (Guid id, TodoService todoService) =>
        {
            var result = await todoService.Remove(id);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName(ApiEndpointNames.DeleteTodo);
        
        concreteTodoGroup.MapPut("", async (
            Guid id, 
            TodoRequest todoRequest, 
            TodoService todoService) =>
        {
            var todoDetails = todoRequest.ToDetails();
            var updatedTodo = await todoService.Update(id, todoDetails);
            return updatedTodo != null ? Results.Ok(updatedTodo) : Results.NotFound();
        })
        .WithName(ApiEndpointNames.EditTodo);
        
        concreteTodoGroup.MapPut("/complete", async (Guid id, TodoService todoService) =>
        {
            var result = await todoService.Complete(id);
            return result ? Results.Ok() : Results.NotFound();
        })
        .WithName(ApiEndpointNames.CompleteTodo);
        
        concreteTodoGroup.MapPut("/uncomplete", async (Guid id, TodoService todoService) =>
        {
            var result = await todoService.Uncomplete(id);
            return result ? Results.Ok() : Results.NotFound();
        })
        .WithName(ApiEndpointNames.UncompleteTodo);
        

    }
}