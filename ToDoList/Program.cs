using ToDoList.Dtos;
using ToDoList.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultFiles(); 
app.UseStaticFiles();

app.MapPost("/todos", (CreateTodoRequest createTodoRequest, ITodoRepository repository) =>
{
    var todoDetails = createTodoRequest.ToTodoDetails();
    var todo = repository.Create(todoDetails);
    return Results.CreatedAtRoute("GetTodo", new {id = todo.Id} , todo);
}).WithName("CreateTodo");

app.MapGet("/todos/{id:int}", (int id, ITodoRepository repository) =>
{
    var todo = repository.Get(id);
    return todo != null ? Results.Ok(todo) : Results.NotFound();
}).WithName("GetTodo");

app.MapDelete("/todos/{id:int}", (int id, ITodoRepository repository) =>
{
    var todo = repository.Remove(id);
    return todo ? Results.NoContent() : Results.NotFound();
}).WithName("DeleteTodo");

app.MapPut("/todos/{id:int}", (int id, CreateTodoRequest createTodoRequest, ITodoRepository repository) =>
{
    var todoDetails = createTodoRequest.ToTodoDetails();
    var todo = repository.Update(id, todoDetails);
    return todo != null ? Results.Ok(todo) : Results.NotFound();
}).WithName("EditTodo");

app.MapGet("/todos", (ITodoRepository repository) => Results.Ok(repository.GetAll())).WithName("GetAllTodo");

app.Run();
