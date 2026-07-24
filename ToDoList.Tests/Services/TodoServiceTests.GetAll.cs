using FluentAssertions;
using ToDoList.Dtos;
using Xunit;

namespace ToDoList.Tests.Services;

public partial class TodoServiceTests
{
    // GetAll
    // - [Happy] В базе есть задачи нескольких пользователей -> Возвращены задачи переданого пользователя
    // - [Happy] У юзера нет задач -> []
    // - [Happy] У юзера есть задача с тегами -> Теги корректно получены
    
    [Fact]
    public async Task GetAll_FewUsersHaveTasks_ReturnsOnlyCurrentUserTasks()
    {
        // Arrange
        var currentUserId = Guid.NewGuid();
        var foreignUserId = Guid.NewGuid();

        await _todoService.Create(CreateTestTodoDetails(userId: currentUserId, title: "CurrentUserTitle"), new List<Guid>());
        await _todoService.Create(CreateTestTodoDetails(userId: foreignUserId), new List<Guid>());
        
        // Act
        var res = await _todoService.GetAll(currentUserId, string.Empty, null, new GetTodosQuery());
        
        // Assert
        res.Should().NotBeNull();
        res.Should().HaveCount(1);
        res[0].Title.Should().Be("CurrentUserTitle");
    }
    
    [Fact]
    public async Task GetAll_UserDontHaveTasks_ReturnsEmptyCollection()
    {
        // Arrange
        var currentUserId = Guid.NewGuid();
        
        // Act
        var res = await _todoService.GetAll(currentUserId, string.Empty, null, new GetTodosQuery());
        
        // Assert
        res.Should().NotBeNull();
        res.Should().HaveCount(0);
    }
    
    [Fact]
    public async Task GetAll_UserHaveTaskWithTags_ReturnsTasksWithValidTags()
    {
        // Arrange
        var currentUserId = Guid.NewGuid();
        var tag = await CreateAndSaveTestTagAsync(currentUserId);
        await _todoService.Create(CreateTestTodoDetails(userId: currentUserId, title: "CurrentUserTitle"), new List<Guid> {tag.Id});
        
        // Act
        var res = await _todoService.GetAll(currentUserId, string.Empty, null, new GetTodosQuery());
        
        // Assert
        res.Should().NotBeNull();
        res.Should().HaveCount(1);
        res[0].TagIds.Should().HaveCount(1).And.Contain(tag.Id);
    }
}