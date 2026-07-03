using ToDoList.Models;

namespace ToDoList.Services;

public interface ITodoRepository
{
    public TodoItem Create(TodoDetails todoDetails);
    public bool Remove(int id);
    public TodoItem? Get(int id);
    public IReadOnlyList<TodoItem> GetAll();
    public TodoItem? Update(int id, TodoDetails todoDetails);
}