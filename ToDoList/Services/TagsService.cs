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

    public async Task<Tag> Create(Tag tag, CancellationToken cancellationToken = default)
    {
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync(cancellationToken);
        return tag;
    }

    public async Task<Tag?> Get(Guid tagId, CancellationToken cancellationToken = default)
    {
        var tag = await _context.Tags.FindAsync(new object?[] { tagId }, cancellationToken);
        return tag;
    }

    public async Task<IReadOnlyList<Tag>> GetAll(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Tags.Where(t => t.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var tag = await Get(id, cancellationToken);
        if (tag == null)
        {
            return;
        }
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
