using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models;

public class TodoItem(string title, string description)
{
    public int Id { get; init; }
    
    public string Title { get; private set; } = title;

    public string Description { get; private set; } = description;
    
    public bool IsDone { get; private set; }
    
    public DateTime CreatedAt { get; init; }

    public void MarkAsCompleted() => IsDone = true;
    public void MarkAsUncompleted() => IsDone = false;
}