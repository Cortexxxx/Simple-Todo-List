using ToDoList.Models;
using ToDoList.Services;
using FluentAssertions;
using Xunit;

namespace ToDoList.Tests.Services;

public class TodoServiceTests
{
    [Fact]
    public async Task Add_ValidTodo_MustAddToStorage()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();
        var todoService = new TodoService(context);
        var startCount = context.Todos.Count();
        var todo = new TodoDetails()
        {
            Title = "Test title",
        };
        
        // Act
        var res = await todoService.Create(todo, new List<Guid>());
        
        // Assert

        context.Todos.Should().HaveCount(startCount + 1, "because we added one todo");
        res.Should().NotBeNull();
        res.Title.Should().Be("Test title");
    }
}