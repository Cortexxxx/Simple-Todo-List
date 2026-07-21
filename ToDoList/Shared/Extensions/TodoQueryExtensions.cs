using System.Linq.Expressions;
using ToDoList.Dtos;
using ToDoList.Models;
using ToDoList.Models.Enums;

namespace ToDoList.Shared.Extensions;

public static class TodoQueryExtensions
{
    public static IQueryable<TodoItem> FilterByUser(this IQueryable<TodoItem> query, Guid userId) 
        => query.Where(t => t.UserId == userId);

    public static IQueryable<TodoItem> FilterByFolder(this IQueryable<TodoItem> query, string? folderName, string? currentDateTime)
    {
        if (folderName != "deleted")
        {
            query = query.Where(t => !t.IsDeleted);
        }

        var userDate = (DateTime.TryParse(currentDateTime, out var parsed) ? parsed : DateTime.UtcNow).Date;

        return folderName?.ToLower() switch
        {
            "today" => query.Where(t => t.ScheduledDate != null && 
                (t.ScheduledDate.Value.Date == userDate || (!t.IsDone && t.ScheduledDate.Value.Date < userDate))),

            "tomorrow" => query.Where(t => t.ScheduledDate != null && 
                t.ScheduledDate.Value.Date == userDate.AddDays(1)),

            "inbox" => query.Where(t => t.ScheduledDate == null),

            "deleted" => query.Where(t => t.IsDeleted),

            "completed" => query.Where(t => t.IsDone),

            _ => FilterByTag(query, folderName)
        };
    }

    public static IQueryable<TodoItem> ApplySorting(this IQueryable<TodoItem> query, GetTodosQuery sortQuery)
    {
        var isDesc = sortQuery.IsDescending;

        if (string.Equals(sortQuery.SortBy, "createdat", StringComparison.OrdinalIgnoreCase))
        {
            return isDesc 
                ? query.OrderByDescending(t => t.CreatedAt)
                : query.OrderBy(t => t.CreatedAt);
        }
        
        return isDesc 
            ? query.OrderByDescending(t => t.Priority).ThenByDescending(t => t.CreatedAt)
            : query.OrderBy(t => t.Priority).ThenByDescending(t => t.CreatedAt);
    }
    private static IQueryable<TodoItem> FilterByTag(IQueryable<TodoItem> query, string? folderName)
    {
        if (string.IsNullOrEmpty(folderName)) return query;

        var rawTag = folderName.StartsWith("tag-") ? folderName["tag-".Length..] : folderName;

        return Guid.TryParse(rawTag, out var tagId)
            ? query.Where(t => t.Tags.Any(tag => tag != null && tag.Id == tagId))
            : query;
    }
}