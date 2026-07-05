namespace ToDoList.Models;

public record TodoDetails()
{
    public Guid UserId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
}