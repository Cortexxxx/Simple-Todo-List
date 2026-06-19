using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos;

public record struct CreateTodoRequest
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Title { get; set; }
    
    [MaxLength(300)]
    public string Description { get; set; }
}