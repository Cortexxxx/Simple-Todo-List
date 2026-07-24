using ToDoList.Models;
using ToDoList.Services;
using ToDoList.Infrastructure.Data;
using Xunit;

namespace ToDoList.Tests.Services;

public partial class TodoServiceTests : IAsyncLifetime
{
    private readonly AppDbContext _context;
    private readonly TodoService _todoService;

    public TodoServiceTests()
    {
        _context = TestDbContextFactory.Create();
        _todoService = new TodoService(_context);
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await _context.DisposeAsync();

    private TodoDetails CreateTestTodoDetails(string title = "Test title", Guid? userId = null)
    {
        return new TodoDetails
        {
            Title = title,
            UserId = userId ?? Guid.NewGuid(),
            Description = "Default Description"
        };
    }

    private async Task<Tag> CreateAndSaveTestTagAsync(Guid userId, string name = "Test Tag")
    {
        var tag = new Tag
        {
            Id = Guid.NewGuid(),
            Name = name,
            UserId = userId
        };

        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        return tag;
    }
}