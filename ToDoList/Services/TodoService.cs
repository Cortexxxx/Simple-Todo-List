using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Dtos;
using ToDoList.Mappings;
using ToDoList.Models;

namespace ToDoList.Services;

public class TodoService
{
    private AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> Create(TodoDetails todoDetails)
    {
        var todo = new TodoItem(todoDetails);
        await _context.AddAsync(todo);
        await _context.SaveChangesAsync();
        return todo;
    }

    public async Task<bool> Remove(int id)
    {
        var todo = await Get(id);

        if (todo == null)
        {
            return false;
        }

        todo.Delete();
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<TodoItem?> Get(int id)
    {
        var todo = await _context.Todos.FindAsync(id);

        return (bool)todo?.IsDeleted ? null : todo;
    }

    public async Task<IReadOnlyList<TodoResponse>> GetAll()
    {
        return await _context.Todos.Where(t => !t.IsDeleted).Select(t => t.ToResponse()).ToListAsync();
    }

    public async Task<TodoItem?> Update(int id, TodoDetails todoDetails)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null || todo.IsDeleted)
        {
            return null;
        }

        todo.UpdateDetails(todoDetails);
        await _context.SaveChangesAsync();
        
        return todo;
    }
}