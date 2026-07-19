namespace ToDoList.Dtos;

public record TagResponse()
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }   
}