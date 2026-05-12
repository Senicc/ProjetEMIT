using System.ComponentModel.DataAnnotations;

namespace ProjetEMIT.ViewModels;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Nom")]
    public string Nom { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Prénom")]
    public string Prenom { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
