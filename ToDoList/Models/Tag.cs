
namespace ToDoList.Models;

public class Tag
{
    public Tag()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Color { get; set; } = "#808080";
    public string Name { get; set; }
    public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
}