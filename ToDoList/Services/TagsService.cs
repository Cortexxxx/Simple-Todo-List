using Microsoft.EntityFrameworkCore;
using ToDoList.Dtos;
using ToDoList.Infrastructure.Data;
using ToDoList.Models;

namespace ToDoList.Services;

public class TagsService
{
    private AppDbContext _context;

    public TagsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tag> Create(TagRequest request, Guid userId)
    {
        var tag = new Tag
        {
            UserId = userId,
            Color = request.Color,
            Name = request.Name,
        };
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        return tag;
    }
    
    public async Task<Tag?> Get(Guid tagId)
    {
        var tag = await _context.Tags.FindAsync(tagId);
        return tag;
    }

    public async Task<IReadOnlyList<Tag>> GetAll(Guid userId)
    {
        return await _context.Tags.Where(t => t.UserId == userId).ToListAsync();
    }

    public async Task Delete(Guid id)
    {
        _context.Tags.Remove(await Get(id));
        await _context.SaveChangesAsync();
    }
}