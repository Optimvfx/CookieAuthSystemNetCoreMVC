using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entities.Configuration;

namespace DAL.Entities;

[Table("Roles")]
public class Role
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(RoleConfiguration.MaxNameLength)]
    [MinLength(RoleConfiguration.MinNameLength)]
    public string Name { get; set; } = null!;
    
    [Required]
    public virtual ICollection<User> Users { get; set; } = null!;
}