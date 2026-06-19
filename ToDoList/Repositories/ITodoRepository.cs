using ToDoList.Dtos;
using ToDoList.Models;

namespace ToDoList.Repositories;

public interface ITodoRepository
{
    public TodoItem Create(CreateTodoRequest todoDto);
    public bool Remove(int id);
    public TodoItem? Get(int id);
    public IReadOnlyList<TodoItem> GetAll();
}