using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DAL.Entities;
using DAL.Entities.Configuration;

namespace BLL.Models.Tasks.Request;

public class TaskCreateRequest
{
    [StringLength(TaskConfiguration.TitleMaxLength, MinimumLength = TaskConfiguration.TitleMinLength,
        ErrorMessage = "Длина должна быть от {1} до {2} символов")]
    [Required(ErrorMessage = "Поле является обязательным")]
    public string Title { get; set; } = null!;

    [StringLength(TaskConfiguration.DescriptionMaxLength, MinimumLength = TaskConfiguration.DescriptionMinLength,
        ErrorMessage = "Длина должна быть от {1} до {2} символов")]
    public string? Description { get; set; } = null;
    
    [Required(ErrorMessage = "Поле является обязательным")]
    public DateTime DueDate { get; set; }

}