using System.ComponentModel.DataAnnotations;

namespace Deathwrap.GerageGenius.FrontAPI.Models;

public class RegisterRequest
{
    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; }
    
    [Required] 
    [Display(Name = "Email")] 
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;
}