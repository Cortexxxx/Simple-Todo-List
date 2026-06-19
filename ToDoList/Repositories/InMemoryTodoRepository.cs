using ToDoList.Dtos;
using ToDoList.Models;

namespace ToDoList.Repositories;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<TodoItem> _todos = [];
    private int _lastAddedId = -1;
    public TodoItem Create(CreateTodoRequest todoDto)
    {
        var todo = new TodoItem(todoDto.Title, todoDto.Description)
        {
            Id = ++_lastAddedId, // Warning: no concurrency
            CreatedAt = DateTime.UtcNow
        };
        _todos.Add(todo);
        
        return todo;
    }

    public bool Remove(int id)
    {
        var todo = Get(id);
        if (todo == null) return false;
        
        _todos.Remove(todo);
        
        return true;
    }

    public TodoItem? Get(int id)
    {
        return _todos.Find(t => t.Id == id);
    }

    public IReadOnlyList<TodoItem> GetAll()
    {
        return _todos.AsReadOnly();
    }
}