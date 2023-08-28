namespace BLL.Models.Tasks.View;

public class TaskViewModel
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public string? Description { get; set; } = null;
    
    public DateTime DueDate { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public Guid AuthorId { get; set; }
}