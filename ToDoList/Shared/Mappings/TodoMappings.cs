using ToDoList.Dtos;
using ToDoList.Models;

namespace ToDoList.Shared.Mappings;

public static class TodoMappings
{
    public static TodoResponse ToResponse(this TodoItem todoItem)
    {
        return new TodoResponse()
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            Priority = todoItem.Priority,
            IsDone = todoItem.IsDone,
            CreatedAt = todoItem.CreatedAt,
            IsDeleted = todoItem.IsDeleted,
            Deadline = todoItem.Deadline,
            ScheduledDate = todoItem.ScheduledDate, 
            TagIds = todoItem.Tags.Select(t => t.Id).ToList()
        };
    }
    
    public static TodoDetails ToDetails(this TodoRequest todoRequest, Guid userId)
    {
        var details = todoRequest.ToDetails();
        details.UserId = userId;
        return details;
    }
    
    public static TodoDetails ToDetails(this TodoRequest todoRequest)
    {
        var details = new TodoDetails()
        {
            Title = todoRequest.Title,
            Description = todoRequest.Description,
            Priority = todoRequest.Priority,
            ScheduledDate = todoRequest.ScheduledDate,
            Deadline = todoRequest.Deadline,
        };
        return details;
    }
}