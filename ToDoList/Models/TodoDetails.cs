using ToDoList.Models.Enums;

namespace ToDoList.Models;

public record TodoDetails
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? Deadline { get; set; }
    public ICollection<Tag?> Tags { get; set; } = new List<Tag?>();
}