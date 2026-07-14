using ToDoList.Models;

namespace ToDoList.Services;

public interface ITodoRepository
{
    public TodoItem Create(TodoDetails todoDetails);
    public bool Remove(Guid id);
    public TodoItem? Get(Guid id);
    public IReadOnlyList<TodoItem> GetAll();
    public TodoItem? Update(Guid id, TodoDetails todoDetails);
}