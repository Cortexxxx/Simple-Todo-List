using ToDoList.Dtos;
using ToDoList.Services;
using ToDoList.Shared.Constants;
using ToDoList.Shared.Mappings;

namespace ToDoList.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/todos").RequireAuthorization();

        group.MapPost("/", async (TodoRequest todoRequest, TodoService todoService) =>
        {
            var todo = await todoService.Create(todoRequest.ToDetails());
            return Results.CreatedAtRoute(ApiEndpointNames.GetTodo, new {id = todo.Id} , todo);
        })
        .WithName(ApiEndpointNames.CreateTodo);
        
        // #################
        
        group.MapGet("/{id:int}", async (int id, TodoService todoService) =>
        {
            var todo = await todoService.Get(id);
            return todo != null ? Results.Ok(todo) : Results.NotFound();
        })
        .WithName(ApiEndpointNames.GetTodo);
        
        group.MapDelete("/{id:int}", async (int id, TodoService todoService) =>
        {
            var result = await todoService.Remove(id);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName(ApiEndpointNames.DeleteTodo);
        
        group.MapPut("/{id:int}", async (int id, TodoRequest todoRequest, TodoService todoService) =>
        {
            var todoDetails = todoRequest.ToDetails();
            var updatedTodo = await todoService.Update(id, todoDetails);
            return updatedTodo != null ? Results.Ok(updatedTodo) : Results.NotFound();
        })
        .WithName(ApiEndpointNames.EditTodo);
        
        group.MapPut("/{id:int}/complete", async (int id, TodoService todoService) =>
        {
            var result = await todoService.Complete(id);
            return result ? Results.Ok() : Results.NotFound();
        })
        .WithName(ApiEndpointNames.CompleteTodo);
        
        group.MapPut("/{id:int}/uncomplete", async (int id, TodoService todoService) =>
        {
            var result = await todoService.Uncomplete(id);
            return result ? Results.Ok() : Results.NotFound();
        })
        .WithName(ApiEndpointNames.UncompleteTodo);
        
        group.MapGet("", async (TodoService todoService) =>
        {
            var todos = await todoService.GetAll();
            return Results.Ok(todos);
        })
        .WithName(ApiEndpointNames.GetAllTodos);
    }
}