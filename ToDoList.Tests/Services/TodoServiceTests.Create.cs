using ToDoList.Models;
using FluentAssertions;
using Xunit;

namespace ToDoList.Tests.Services;

public partial class TodoServiceTests
{
    // Кейсы для метода Create:
    // + [Happy]  Передан валидный список тегов или пустой список тегов -> Задача создается с нужными тегами
    // + [Sad]  Передана задача с невалидным списком тегов -> ArgumentException
    // + [Sad]  Передан тег принадлежащий другому пользователю -> ArgumentException
    // + [Sad]  Переданы два одинаковых Guid тега -> ArgumentException
    // + [Sad]  todoDetails == null -> ArgumentException
    // + [Sad]  tagIds == null -> ArgumentException

    public static IEnumerable<object[]> GetTagTestValidData => new List<object[]>
    {
        new object[] { 2 },
        new object[] { 0 }
    };

    [Theory]
    [MemberData(nameof(GetTagTestValidData))]
    public async Task Create_TodoWithValidTags_MustAddTodoToStorageWithTags(int tagsCount)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var tags = new List<Tag>();

        for (var i = 0; i < tagsCount; i++)
        {
            tags.Add(await CreateAndSaveTestTagAsync(userId, $"TestTag_{i}"));
        }

        var todoDetails = CreateTestTodoDetails(userId: userId);
        var tagIds = tags.Select(t => t.Id).ToList();

        // Act
        var res = await _todoService.Create(todoDetails, tagIds);

        // Assert
        res.Should().NotBeNull();
        res.Id.Should().NotBeEmpty();
        res.Title.Should().Be("Test title");
        res.Tags.Should().BeEquivalentTo(tags);
    }

    [Fact]
    public async Task Create_TodoWithUnknownTags_MustThrowArgumentException()
    {
        // Arrange
        var todoDetails = CreateTestTodoDetails();
        var invalidTagIds = new List<Guid> { Guid.NewGuid() };

        // Act
        var act = async () => await _todoService.Create(todoDetails, invalidTagIds);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }


    [Fact]
    public async Task Create_TodoWithForeignTag_MustThrowArgumentException()
    {
        // Arrange
        var currentUserId = Guid.NewGuid();
        var foreignUser = Guid.NewGuid();
        var todoDetails = CreateTestTodoDetails(userId: currentUserId);
        var foreignTag = await CreateAndSaveTestTagAsync(foreignUser);
        
        // Act
        var act = () => _todoService.Create(todoDetails, new List<Guid> { foreignTag.Id });
        
        // Assert

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task Create_TodoWithSameTags_MustThrowArgumentException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var todoDetails = CreateTestTodoDetails(userId: userId);
        var tag = await CreateAndSaveTestTagAsync(userId);
        
        // Act
        var act = () => _todoService.Create(todoDetails, new List<Guid> { tag.Id, tag.Id });
        
        // Assert

        await act.Should().ThrowAsync<ArgumentException>();
    }
    
    [Fact]
    public async Task Create_TodoWithNullTodoDetails_MustThrowArgumentNullException()
    {
        // Arrange
        
        // Act
        var act = () => _todoService.Create(null, new List<Guid>());
        
        // Assert

        await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("todoDetails");
    }
    
    [Fact]
    public async Task Create_TodoWithNullTags_MustThrowArgumentNullException()
    {
        // Arrange
        
        // Act
        var act = () => _todoService.Create(new TodoDetails(), null);
        
        // Assert

        await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("tagIds");
    }
}