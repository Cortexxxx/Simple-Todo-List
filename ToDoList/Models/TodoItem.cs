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
    }
    
    public int Id { get; private set; }
    
    public Guid UserId { get; private set; }
    public string Title { get; private set; }

    public string Description { get; private set; }
    
    public bool IsDone { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public bool IsDeleted { get; private set; } = false;

    public void MarkAsCompleted() => IsDone = true;
    public void MarkAsUncompleted() => IsDone = false;

    public void Delete() => IsDeleted = true;

    public void UpdateDetails(TodoDetails details)
    {
        Title = details.Title;
        Description = details.Description;
    }
}