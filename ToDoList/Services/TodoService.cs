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

    public async Task<TodoItem> Create(TodoDetails todoDetails, ICollection<Guid> tagIds, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(todoDetails);
        ArgumentNullException.ThrowIfNull(tagIds);

        if (tagIds.Count != 0)
        {
            await ParseTags(todoDetails, tagIds, cancellationToken);
        }

        var todo = new TodoItem(todoDetails);
        _context.Add(todo);
        await _context.SaveChangesAsync(cancellationToken);
        return todo;
    }

    private async Task ParseTags(TodoDetails todoDetails, ICollection<Guid> tagIds, CancellationToken cancellationToken)
    {
        var tags = await _context.Tags
            .Where(t =>  tagIds.Contains(t.Id) && t.UserId == todoDetails.UserId)
            .ToListAsync(cancellationToken);

        if (tagIds.Count != tags.Count)
        {
            throw new ArgumentException(message: "Invalid argument tags");
        }

        todoDetails.Tags = tags;
    }

    public async Task<bool> Remove(Guid id, CancellationToken cancellationToken = default)
    {
        var todo = await Get(id, cancellationToken);

        if (todo is null || todo.IsDeleted)
        {
            return false;
        }

        todo.Delete();
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<TodoItem?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var todo = await _context.Todos.FindAsync(new object?[] { id }, cancellationToken);

        return todo is { IsDeleted: true } ? null : todo;
    }

    public async Task<IReadOnlyList<TodoResponse>> GetAll(
        Guid userId,
        string folder,
        string? currentDateTime,
        GetTodosQuery query,
        CancellationToken cancellationToken = default)
    {

        var tasksQuery = _context.Todos
            .Include(t => t.Tags)
            .FilterByUser(userId)
            .FilterByFolder(folder, currentDateTime)
            .ApplySorting(query);
        return await tasksQuery.Select(t => t.ToResponse()).ToListAsync(cancellationToken);
    }

    public async Task<TodoItem?> Update(Guid id, TodoDetails todoDetails, CancellationToken cancellationToken = default)
    {
        var todo = await Get(id, cancellationToken);

        if (todo is null)
        {
            return null;
        }

        todo.UpdateDetails(todoDetails);
        await _context.SaveChangesAsync(cancellationToken);

        return todo;
    }

    public async Task<bool> Complete(Guid id, CancellationToken cancellationToken = default)
    {
        return await SetCompletionStatus(id, true, cancellationToken);
    }

    public async Task<bool> Uncomplete(Guid id, CancellationToken cancellationToken = default)
    {
        return await SetCompletionStatus(id, false, cancellationToken);
    }

    private async Task<bool> SetCompletionStatus(Guid id, bool isCompleted, CancellationToken cancellationToken)
    {
        var task = await Get(id, cancellationToken);

        if (task is null)
        {
            return false;
        }

        if (isCompleted)
            task.MarkAsCompleted();
        else
            task.MarkAsUncompleted();

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}