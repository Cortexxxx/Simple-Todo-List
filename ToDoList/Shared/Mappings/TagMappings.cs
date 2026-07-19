using ToDoList.Dtos;
using ToDoList.Models;

namespace ToDoList.Shared.Mappings;

public static class TagMappings
{
    public static Tag ToTagModel(this TagRequest request, Guid userId)
    {
        return new Tag()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Color = request.Color,
            UserId = userId,
            TodoItems = new List<TodoItem>()
        };
    }
    
    public static TagResponse ToResponse(this Tag tag)
    {
        return new TagResponse()
        {
            Id = tag.Id,
            Name = tag.Name,
            Color = tag.Color
        };
    }
}