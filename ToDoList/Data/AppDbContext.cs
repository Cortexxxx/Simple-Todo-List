using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<TodoItem> Todos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>(b =>
        {
            b.Property(t => t.Title);
            b.Property(t => t.Description);
            b.Property(t => t.IsDone);
            b.Property(t => t.CreatedAt);
            b.Property(t => t.IsDeleted);
        });
    }
}