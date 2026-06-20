using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models;

public class TodoItem(TodoDetails details)
{
    public int Id { get; init; }
    
    public string Title { get; private set; } = details.Title;

    public string Description { get; private set; } = details.Description;
    
    public bool IsDone { get; private set; }
    
    public DateTime CreatedAt { get; init; }

    public void MarkAsCompleted() => IsDone = true;
    public void MarkAsUncompleted() => IsDone = false;

    public void UpdateDetails(TodoDetails details)
    {
        Title = details.Title;
        Description = details.Description;
    }
}