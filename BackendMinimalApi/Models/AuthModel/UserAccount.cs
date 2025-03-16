using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendMinimalApi.Models.AuthModel;

[Table("user_account")]
public class UserAccount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Column("user_name")]
    [Required]
    [MaxLength(20)]
    public string Username { get; set; } = string.Empty;
    
    [Column("user_password")]
    [Required]
    [MaxLength(256)]
    public string Password { get; set; } = string.Empty;
}