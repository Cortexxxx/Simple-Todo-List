using FluentAssertions;
using Xunit;

namespace ToDoList.Tests.Services;

public partial class TodoServiceTests
{
    
    // Remove
    // + [Happy] Передан валидный ID задачи, задача еще не была удалена -> Задача помечена удаленной + true
    // + [Sad] Передан валидный ID задачи, задача уже была удалена -> false
    // + [Sad] Передана несуществующая задача -> false

    [Fact]
    public async Task Remove_ExistingTodo_MustSoftDeleteAndReturnTrue()
    {
        // Arrange
        var todo = CreateTestTodoDetails();
        var todoItem = await _todoService.Create(todo, new List<Guid>());
        
        // Act
        var res =  await _todoService.Remove(todoItem.Id);

        // Assert
        res.Should().Be(true);
        var dbTodo = await _context.Todos.FindAsync(todoItem.Id);
        dbTodo!.IsDeleted.Should().BeTrue();
    }
    
    [Fact]
    public async Task Remove_AlreadyDeletedTodo_MustReturnFalse()
    {
        // Arrange
        var todo = CreateTestTodoDetails();
        var todoItem = await _todoService.Create(todo, new List<Guid>());
        await _todoService.Remove(todoItem.Id);

        // Act
        var res =  await _todoService.Remove(todoItem.Id);

        // Assert
        res.Should().Be(false);
    }
    
    [Fact]
    public async Task Remove_NonexistingTodo_MustReturnFalse()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var res = await _todoService.Remove(nonExistingId);

        // Assert
        res.Should().Be(false);
    }
}