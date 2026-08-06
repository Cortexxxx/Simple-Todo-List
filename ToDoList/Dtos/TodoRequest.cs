using ToDoList.Models.Enums;

namespace ToDoList.Dtos;

public record TodoRequest
{
    public string Title { get; set; } = string.Empty; 
    
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; }

    public DateTime? ScheduledDate { get; set; } = null;
    public DateTime? Deadline { get; set; } = null;
    public ICollection<Guid> TagIds { get; set; } = new List<Guid>();
}