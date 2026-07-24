using FluentAssertions;
using Xunit;

namespace ToDoList.Tests.Services;

public partial class TodoServiceTests
{
    // Complete
    // [Happy] Передана существующая задача -> Задача помечена выполненной + true
    // [Sad] Передана несуществующая задача -> false

    [Fact]
    public async Task Complete_ExistingTask_MarkTaskAsCompletedAndReturnTrue()
    {
        // Arrange
        var todo = await _todoService.Create(CreateTestTodoDetails(), new List<Guid>());
        
        // Act
        var res = await _todoService.Complete(todo.Id);
        
        // Assert
        res.Should().Be(true);
        (await _context.Todos.FindAsync(todo.Id))!.IsDone.Should().Be(true);
    }
    
    [Fact]
    public async Task Complete_NotExistingTask_ReturnFalse()
    {
        // Arrange
        
        // Act
        var res = await _todoService.Complete(Guid.NewGuid());
        
        // Assert
        res.Should().Be(false);
    }
    
    // Complete
    // [Happy] Передана существующая задача -> Задача помечена невыполненной + true
    // [Sad] Передана несуществующая задача -> false

    [Fact]
    public async Task Uncomplete_ExistingTask_MarkTaskAsUncompletedAndReturnTrue()
    {
        // Arrange
        var todo = await _todoService.Create(CreateTestTodoDetails(), new List<Guid>());
        await _todoService.Complete(todo.Id);
        // Act
        var res = await _todoService.Uncomplete(todo.Id);
        
        // Assert
        res.Should().Be(true);
        (await _context.Todos.FindAsync(todo.Id))!.IsDone.Should().Be(false);
    }
    
    [Fact]
    public async Task Uncomplete_NotExistingTask_ReturnFalse()
    {
        // Arrange
        
        // Act
        var res = await _todoService.Uncomplete(Guid.NewGuid());
        
        // Assert
        res.Should().Be(false);
    }
}