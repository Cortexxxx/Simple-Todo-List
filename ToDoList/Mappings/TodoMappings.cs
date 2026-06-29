using ToDoList.Dtos;
using ToDoList.Models;

namespace ToDoList.Mappings;

public static class TodoMappings
{
    public static TodoResponse ToResponse(this TodoItem todoItem)
    {
        return new TodoResponse()
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            IsDone = todoItem.IsDone,
            CreatedAt = todoItem.CreatedAt,
            IsDeleted = todoItem.IsDeleted
        };
    }
    
    public static TodoDetails ToDetails(this TodoRequest todoRequest)
    {
        return new TodoDetails
        {
            Title = todoRequest.Title,
            Description = todoRequest.Description
        };
    }
}