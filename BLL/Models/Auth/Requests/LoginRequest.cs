using System.ComponentModel.DataAnnotations;
using DAL.Entities.Configuration;

namespace BLL.Models.Auth;

public class LoginRequest
{
    [EmailAddress(ErrorMessage = "Поле должно являться почтой")]
    [MaxLength(MailConfiguration.MaxLength, ErrorMessage = "Почта должна быть меньше {1} символов")]
    [Required(ErrorMessage = "Поле является обязательным")]
    public string Email { get; set; } = null!;


    [DataType(DataType.Password)]
    [StringLength(PasswordConfiguration.MaxLength, MinimumLength = PasswordConfiguration.MinLength,
        ErrorMessage = "Длина должна быть от {1} до {2} символов")]
    [Required(ErrorMessage = "Поле является обязательным")]
    public string Password { get; set; } = null!;
}