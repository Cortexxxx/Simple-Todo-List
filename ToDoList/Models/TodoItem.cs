using ToDoList.Models.Enums;

namespace ToDoList.Models;

public class TodoItem
{
    // ReSharper disable once UnusedMember.Local
    // Entity framework uses this constructor
    private TodoItem()
    {
        Title = null!;
        Description = null!;
    }
    public TodoItem(TodoDetails details)
    {
        UserId = details.UserId;
        Title = details.Title;
        Description = details.Description;
        Priority = details.Priority;
        CreatedAt = DateTime.UtcNow;
        ScheduledDate = details.ScheduledDate;
        Deadline = details.Deadline;
        Tags = details.Tags;
    }
    
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    public string Title { get; private set; }

    public string Description { get; private set; }
    public Priority Priority { get; private set; }
    
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    
    public bool IsDone { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime? ScheduledDate { get; private set; }
    
    public DateTime? Deadline { get; private set; }

    public bool IsDeleted { get; private set; }

    public void MarkAsCompleted() => IsDone = true;
    public void MarkAsUncompleted() => IsDone = false;

    public void Delete() => IsDeleted = true;

    public void UpdateDetails(TodoDetails details)
    {
        Title = details.Title;
        Description = details.Description;
        Deadline = details.Deadline;
        ScheduledDate = details.ScheduledDate;
        Priority = details.Priority;
        Tags.Clear();
        foreach (var tag in details.Tags)
        {
            Tags.Add(tag);
        }
    }
}