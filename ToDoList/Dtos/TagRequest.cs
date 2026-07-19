namespace ToDoList.Dtos;

public record TagRequest()
{
    public string Name { get; set; }
    public string Color { get; set; }
}