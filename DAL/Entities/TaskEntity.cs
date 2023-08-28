using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DAL.Entities.Configuration;

namespace DAL.Entities;

[Table("Tasks")]
public class TaskEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required] 
    [MaxLength(TaskConfiguration.TitleMaxLength)]
    [MinLength(TaskConfiguration.TitleMinLength)]
    public string Title { get; set; } = null!;

    [AllowNull] 
    [MaxLength(TaskConfiguration.DescriptionMaxLength)]
    [MinLength(TaskConfiguration.DescriptionMinLength)]
    public string? Description { get; set; } = null;
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Required]
    public bool IsCompleted { get; set; }
    
    [Required]
    public Guid AuthorId { get; set; }
    
    [Required] 
    public User Author { get; set; }
}