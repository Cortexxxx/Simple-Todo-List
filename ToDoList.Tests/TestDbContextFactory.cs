using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Data;

namespace ToDoList.Tests;

public class TestDbContextFactory
{
    public static AppDbContext Create()
    {
        // 1. Создаем соединение с SQLite в оперативной памяти
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        // 2. Настраиваем DbContextOptions для использования SQLite
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        // 3. Создаем контекст и схемы таблиц (OnModelCreating сработает автоматически)
        var context = new AppDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }
}