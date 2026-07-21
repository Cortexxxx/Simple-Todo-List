namespace ToDoList.Dtos;

public record GetTodosQuery(
    string? SortBy = "priority", 
    bool IsDescending = true
);