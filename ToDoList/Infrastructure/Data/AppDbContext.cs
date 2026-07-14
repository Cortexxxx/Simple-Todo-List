using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<TodoItem> Todos { get; set; }
    public DbSet<Tag> Tags { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TodoItem>(b =>
        {
            b.Property(t => t.Title);
            b.Property(t => t.Description);
            b.Property(t => t.IsDone);
            b.Property(t => t.CreatedAt);
            b.Property(t => t.IsDeleted);
            b.Property(t => t.ScheduledDate);
            b.Property(t => t.Deadline);
        });
    }
}