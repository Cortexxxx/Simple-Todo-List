namespace ToDoList.Models;

public record TodoDetails()
{
    public string Title { get; init; }
    public string Description { get; init; }
}