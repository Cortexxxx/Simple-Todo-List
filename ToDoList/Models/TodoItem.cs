namespace ToDoList.Models;

public class TodoItem
{
    private TodoItem() { }
    public TodoItem(TodoDetails details)
    {
        UserId = details.UserId;
        Title = details.Title;
        Description = details.Description;
        CreatedAt = DateTime.UtcNow;
        ScheduledDate = details.ScheduledDate;
        Deadline = details.Deadline;
        Tags = details.Tags;
    }
    
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    public string Title { get; private set; }

    public string Description { get; private set; }
    
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    
    public bool IsDone { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime? ScheduledDate { get; private set; } = null;
    
    public DateTime? Deadline { get; private set; } = null;

    public bool IsDeleted { get; private set; } = false;

    public void MarkAsCompleted() => IsDone = true;
    public void MarkAsUncompleted() => IsDone = false;

    public void Delete() => IsDeleted = true;

    public void UpdateDetails(TodoDetails details)
    {
        Title = details.Title;
        Description = details.Description;
        Deadline = details.Deadline;
        ScheduledDate = details.ScheduledDate;
        Tags.Clear();
        foreach (var tag in details.Tags)
        {
            Tags.Add(tag);
        }
    }
}