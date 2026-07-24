using FluentAssertions;
using Xunit;

namespace ToDoList.Tests.Services;

public partial class TodoServiceTests
{
    // Get
    // - [Happy] Передан id существующей, не удаленной задачи -> Получена задача
    // - [Sad] Передан id существующей, удаленной задачи -> null
    // - [Sad] Передан id несуществующей задачи -> null

    [Fact]
    public async Task Get_ExistingNotRemovedTodo_ReturnsTodo()
    {
        // Arrange
        var todo = await _todoService.Create(CreateTestTodoDetails(), new List<Guid>());
        
        // Act
        var res = await _todoService.Get(todo.Id);
        
        // Assert
        res.Should().BeEquivalentTo(todo);
    }
    
    [Fact]
    public async Task Get_ExistingRemovedTodo_ReturnsNull()
    {
        // Arrange
        var todo = await _todoService.Create(CreateTestTodoDetails(), new List<Guid>());
        await _todoService.Remove(todo.Id);
        // Act
        var res = await _todoService.Get(todo.Id);
        
        // Assert
        res.Should().BeNull();
    }
    [Fact]
    public async Task Get_NotExistingTodo_ReturnsNull()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        // Act
        var res = await _todoService.Get(todoId);
        
        // Assert
        res.Should().BeNull();
    }
}