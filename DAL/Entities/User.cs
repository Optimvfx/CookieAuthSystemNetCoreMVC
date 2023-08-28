using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DAL.Entities.Configuration;

namespace DAL.Entities;

[Table("Users")]
public class User
{
    [Key] public Guid Id { get; set; }

    [EmailAddress]
    [MaxLength(MailConfiguration.MaxLength)]
    [Required]
    public string Email { get; set; } = null!;

    [MaxLength(NickConfiguration.MaxLength)]
    [MinLength(NickConfiguration.MinLength)]
    [Required]
    public string Nick { get; set; } = null!;
    
    [Required] 
    public string PasswordHash { get; set; } = null!;

    [Required] 
    public Guid RoleId { get; set; }

    [Required] 
    public virtual Role Role { get; set; } = null!;
}