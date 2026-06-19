using Microsoft.AspNetCore.Http.HttpResults;
using ToDoList.Dtos;
using ToDoList.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
var app = builder.Build();

app.MapPost("/todos", (CreateTodoRequest createTodoRequest, ITodoRepository repository) =>
{
    var todo = repository.Create(createTodoRequest);
    return Results.CreatedAtRoute("GetTodo", new {id = todo.Id} , todo);
});

app.MapGet("/todos/{id:int}", (int id, ITodoRepository repository) =>
{
    var todo = repository.Get(id);
    return todo == null ? Results.NotFound() : Results.Ok(todo);
}).WithName("GetTodo");

app.MapGet("/todos", (ITodoRepository repository) => Results.Ok(repository.GetAll())).WithName("GetAllTodo");

app.Run();
