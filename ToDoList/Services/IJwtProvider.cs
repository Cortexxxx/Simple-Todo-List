using ToDoList.Models;

namespace ToDoList.Services;

public interface IJwtProvider
{
    string GenerateToken(ApplicationUser user);
}