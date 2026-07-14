using System.ComponentModel.DataAnnotations;
using ToDoList.Models;

namespace ToDoList.Dtos;

public record TodoRequest
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Title { get; set; }
    
    [MaxLength(300)]
    public string Description { get; set; }

    public DateTime? ScheduledDate { get; set; } = null;
    public DateTime? Deadline { get; set; } = null;
    public ICollection<Guid> TagIds { get; set; }
}