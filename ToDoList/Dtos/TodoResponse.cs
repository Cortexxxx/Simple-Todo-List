using ToDoList.Models;

namespace ToDoList.Dtos;

public record TodoResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<Guid> TagIds { get; set; }
    public DateTime? ScheduledDate { get; set; } = null;
    public DateTime? Deadline { get; set; } = null;
    public bool IsDone { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}