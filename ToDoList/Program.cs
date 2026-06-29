using Microsoft.EntityFrameworkCore;
using ToDoList.Constants;
using ToDoList.Data;
using ToDoList.Dtos;
using ToDoList.Mappings;
using ToDoList.Services;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite(connectionString));
builder.Services.AddScoped<TodoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/todos", async (TodoRequest todoRequest, TodoService todoService) =>
{
    var todo = await todoService.Create(todoRequest.ToDetails());
    return Results.CreatedAtRoute(ApiEndpointNames.GetTodo, new {id = todo.Id} , todo);
})
.WithName(ApiEndpointNames.CreateTodo);

// #################

app.MapGet("/todos/{id:int}", async (int id, TodoService todoService) =>
{
    var todo = await todoService.Get(id);
    return todo != null ? Results.Ok(todo) : Results.NotFound();
})
.WithName(ApiEndpointNames.GetTodo);

app.MapDelete("/todos/{id:int}", async (int id, TodoService todoService) =>
{
    var result = await todoService.Remove(id);
    return result ? Results.NoContent() : Results.NotFound();
})
.WithName(ApiEndpointNames.DeleteTodo);

app.MapPut("/todos/{id:int}", async (int id, TodoRequest todoRequest, TodoService todoService) =>
{
    var todoDetails = todoRequest.ToDetails();
    var updatedTodo = await todoService.Update(id, todoDetails);
    return updatedTodo != null ? Results.Ok(updatedTodo) : Results.NotFound();
})
.WithName(ApiEndpointNames.EditTodo);

app.MapGet("/todos", async (TodoService todoService) =>
{
    var todos = await todoService.GetAll();
    return Results.Ok(todos);
})
.WithName(ApiEndpointNames.GetAllTodos);

app.Run();
