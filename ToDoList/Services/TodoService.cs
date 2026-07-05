using Microsoft.EntityFrameworkCore;
using ToDoList.Dtos;
using ToDoList.Infrastructure.Data;
using ToDoList.Models;
using ToDoList.Shared.Mappings;

namespace ToDoList.Services;

public class TodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> Create(TodoDetails todoDetails)
    {
        var todo = new TodoItem(todoDetails);
        _context.Add(todo);
        await _context.SaveChangesAsync();
        return todo;
    }

    public async Task<bool> Remove(int id)
    {
        var todo = await Get(id);

        if (todo is null)
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

        return todo is { IsDeleted: true } ? null : todo;
    }

    public async Task<IReadOnlyList<TodoResponse>> GetAll(Guid userId)
    {
        return await _context.Todos.Where(t => !t.IsDeleted && t.UserId == userId).Select(t => t.ToResponse()).ToListAsync();
    }

    public async Task<TodoItem?> Update(int id, TodoDetails todoDetails)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo is null)
        {
            return null;
        }

        todo.UpdateDetails(todoDetails);
        await _context.SaveChangesAsync();
        
        return todo;
    }

    public async Task<bool> Complete(int id)
    {
        return await SetCompletionStatus(id, true);
    }
    
    public async Task<bool> Uncomplete(int id)
    {
        return await SetCompletionStatus(id, false);
    }

    private async Task<bool> SetCompletionStatus(int id, bool isCompleted)
    {
        var task = await Get(id);

        if (task is null)
        {
            return false;
        }
        
        if (isCompleted)
            task.MarkAsCompleted();
        else 
            task.MarkAsUncompleted();

        await _context.SaveChangesAsync();
        
        return true;
    }
}