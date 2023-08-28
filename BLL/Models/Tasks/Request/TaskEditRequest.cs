using System.ComponentModel.DataAnnotations;
using DAL.Entities.Configuration;

namespace BLL.Models.Tasks.Request;

public class TaskEditRequest
{

    [StringLength(TaskConfiguration.TitleMaxLength, MinimumLength = TaskConfiguration.TitleMinLength,
        ErrorMessage = "Длина должна быть от {1} до {2} символов")]
    public string? Title { get; set; } = null;

    [StringLength(TaskConfiguration.DescriptionMaxLength, MinimumLength = TaskConfiguration.DescriptionMinLength,
        ErrorMessage = "Длина должна быть от {1} до {2} символов")]
    public string? Description { get; set; } = null;

    [Required(ErrorMessage = "Поле является обязательным")]
    public DateTime? DueDate { get; set; } = null;

    public Guid EditingId { get; set; }
}