namespace ToDoList.Dtos;

public record TodoRequest
{
    public string Title { get; set; }
    
    public string Description { get; set; }

    public DateTime? ScheduledDate { get; set; } = null;
    public DateTime? Deadline { get; set; } = null;
    public ICollection<Guid> TagIds { get; set; } = new List<Guid>();
}