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

    public async Task<TodoItem> Create(TodoDetails todoDetails, ICollection<Guid> tagIds)
    {
        if (tagIds.Count != 0)
        {
            var tags = await _context.Tags
                .Where(t =>  tagIds.Contains(t.Id) && t.UserId == todoDetails.UserId)
                .ToListAsync();
            todoDetails.Tags = tags;
        }
        
        var todo = new TodoItem(todoDetails);
        _context.Add(todo);
        await _context.SaveChangesAsync();
        return todo;
    }

    public async Task<bool> Remove(Guid id)
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

    public async Task<TodoItem?> Get(Guid id)
    {
        var todo = await _context.Todos.FindAsync(id);

        return todo is { IsDeleted: true } ? null : todo;
    }

    public async Task<IReadOnlyList<TodoResponse>> GetAll(Guid userId, string folder, string? currentDateTime)
    {
        var query = _context.Todos.Include(t => t.Tags).Where(t => t.UserId == userId);
        var userDatetime = DateTime.UtcNow;
        
        if (!string.IsNullOrEmpty(currentDateTime) && DateTime.TryParse(currentDateTime, out var parsedDate))
        {
            userDatetime = parsedDate;
        }

        if (folder != "deleted")
        {
            query = query.Where(t => !t.IsDeleted);
        }

        var userDate = userDatetime.Date;
        switch (folder)
        {
            case "today":
                query = query.Where(t => t.ScheduledDate != null && (t.ScheduledDate.Value.Date == userDate || (!t.IsDone && t.ScheduledDate.Value < userDate )));
                break;
            case "tomorrow":
                query = query.Where(t => t.ScheduledDate != null && (t.ScheduledDate.Value.Date == userDate.AddDays(1)));
                break;
            case "inbox":
                query = query.Where(t => t.ScheduledDate == null);
                break;
            case "deleted":
                query = query.Where(t => t.IsDeleted);
                break;
            case "completed":
                query = query.Where(t => t.IsDone);
                break;
            default:
                var tagString = folder.StartsWith("tag-") ? folder.Replace("tag-", "") : folder; 
                if (Guid.TryParse(tagString, out var tagGuid))
                {
                    query = query.Where(t => t.Tags.Any(tag => tag.Id == tagGuid));
                }
                break;
        }
        return await query.Select(t => t.ToResponse()).ToListAsync();
    }

    public async Task<TodoItem?> Update(Guid id, TodoDetails todoDetails)
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