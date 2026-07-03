using Microsoft.EntityFrameworkCore;
using ToDoList.Endpoints;
using ToDoList.Infrastructure.Authentication;
using ToDoList.Infrastructure.Data;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite(connectionString));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
        options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();  
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddScoped<TodoService>();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapTodoEndpoints();
app.MapAuthEndpoints();

app.Run();