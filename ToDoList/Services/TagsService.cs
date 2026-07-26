using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Data;
using ToDoList.Models;

namespace ToDoList.Services;

public class TagsService
{
    private readonly AppDbContext _context;

    public TagsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tag> Create(Tag tag)
    {
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
        var tag = await Get(id);
        if (tag == null)
        {
            return;
        }
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
    }
}