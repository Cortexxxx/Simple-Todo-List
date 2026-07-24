using FluentAssertions;
using Xunit;

namespace ToDoList.Tests.Services;

public partial class TodoServiceTests
{
    // Update
    // + [Happy] Передана существующая задача -> Задача изменилась
    // + [Sad] Передана несуществующая задача -> null
    // + [Sad] Передана удаленная задача -> null
    
    [Fact]
    public async Task Update_ExistingNotDeletedTodo_MustUpdateTodo()
    {
        // Arrange
        var todo = await _todoService.Create(CreateTestTodoDetails(), new List<Guid>());
        var newTodoDetails = CreateTestTodoDetails("New title");
        
        // Act

        await _todoService.Update(todo.Id, newTodoDetails);
        
        // Assert
        
        var dbTodo = await _context.Todos.FindAsync(todo.Id);
        dbTodo!.Title.Should().Be("New title");
    }
    
    [Fact]
    public async Task Update_ExistingDeletedTodo_MustReturnNull()
    {
        // Arrange
        var todo = await _todoService.Create(CreateTestTodoDetails(), new List<Guid>());
        await _todoService.Remove(todo.Id);
        var newTodoDetails = CreateTestTodoDetails("New title");
        
        // Act

        var res = await _todoService.Update(todo.Id, newTodoDetails);
        
        // Assert
        
        var dbTodo = await _context.Todos.FindAsync(todo.Id);
        dbTodo!.Title.Should().Be("Test title");
        res.Should().BeNull();
    }
    
    [Fact]
    public async Task Update_NotExistingTodo_MustReturnNull()
    {
        // Arrange
        var newTodoDetails = CreateTestTodoDetails("New title");
        
        // Act

        var res = await _todoService.Update(Guid.NewGuid(), newTodoDetails);
        
        // Assert
        
        res.Should().BeNull();
    }
}