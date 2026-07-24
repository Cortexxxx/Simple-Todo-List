using Microsoft.EntityFrameworkCore;
using ToDoList.Dtos;
using ToDoList.Infrastructure.Data;
using ToDoList.Models;
using ToDoList.Shared.Extensions;
using ToDoList.Shared.Mappings;

namespace ToDoList.Services;

public class TodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> Create(TodoDetails todoDetails, ICollection<Guid> tagIds)
    {
        ArgumentNullException.ThrowIfNull(todoDetails);
        ArgumentNullException.ThrowIfNull(tagIds);
        
        if (tagIds.Count != 0)
        {
            await ParseTags(todoDetails, tagIds);
        }
        
        var todo = new TodoItem(todoDetails);
        _context.Add(todo);
        await _context.SaveChangesAsync();
        return todo;
    }

    private async Task ParseTags(TodoDetails todoDetails, ICollection<Guid> tagIds)
    {
        var tags = await _context.Tags
            .Where(t =>  tagIds.Contains(t.Id) && t.UserId == todoDetails.UserId)
            .ToListAsync();
        
        if (tagIds.Count != tags.Count)
        {
            throw new ArgumentException(message: "Invalid argument tags");
        }
        
        todoDetails.Tags = tags;
    }

    public async Task<bool> Remove(Guid id)
    {
        var todo = await Get(id);

        if (todo is null || todo.IsDeleted)
        {
            return false;
        }

        todo.Delete();
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<TodoItem?> Get(Guid id)
    {
        var todo = await _context.Todos.FindAsync(id);

        return todo is { IsDeleted: true } ? null : todo;
    }

    public async Task<IReadOnlyList<TodoResponse>> GetAll(
        Guid userId, 
        string folder, 
        string? currentDateTime, 
        GetTodosQuery query)
    {

        var tasksQuery = _context.Todos
            .Include(t => t.Tags)
            .FilterByUser(userId)
            .FilterByFolder(folder, currentDateTime)
            .ApplySorting(query);
        return await tasksQuery.Select(t => t.ToResponse()).ToListAsync();
    }

    public async Task<TodoItem?> Update(Guid id, TodoDetails todoDetails)
    {
        var todo = await Get(id);
        
        if (todo is null)
        {
            return null;
        }

        todo.UpdateDetails(todoDetails);
        await _context.SaveChangesAsync();
        
        return todo;
    }

    public async Task<bool> Complete(Guid id)
    {
        return await SetCompletionStatus(id, true);
    }
    
    public async Task<bool> Uncomplete(Guid id)
    {
        return await SetCompletionStatus(id, false);
    }

    private async Task<bool> SetCompletionStatus(Guid id, bool isCompleted)
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