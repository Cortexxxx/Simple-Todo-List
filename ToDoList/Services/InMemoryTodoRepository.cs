using ToDoList.Models;

namespace ToDoList.Services;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<TodoItem> _todos = [];
    private int _lastAddedId = -1;
    public TodoItem Create(TodoDetails todoDetails)
    {
        var todo = new TodoItem(todoDetails);
        _todos.Add(todo);
        return todo;
    }

    public bool Remove(Guid id)
    {
        var todo = Get(id);
        if (todo == null) return false;
        
        _todos.Remove(todo);
        
        return true;
    }

    public TodoItem? Get(Guid id)
    {
        return _todos.Find(t => t.Id == id);
    }

    public IReadOnlyList<TodoItem> GetAll()
    {
        return _todos.AsReadOnly();
    }

    public TodoItem? Update(Guid id, TodoDetails todoDetails)
    {
        var result = _todos.Find(x => x.Id == id);
        result?.UpdateDetails(todoDetails);

        return result;
    }
}