using System.ComponentModel.DataAnnotations;
using ToDoList.Models;

namespace ToDoList.Dtos;

public record CreateTodoRequest
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Title { get; set; }
    
    [MaxLength(300)]
    public string Description { get; set; }

    public TodoDetails ToTodoDetails()
    {
        return new TodoDetails
        {
            Title = Title,
            Description = Description
        };
    }
}