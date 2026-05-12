using System.ComponentModel.DataAnnotations;

namespace ProjetEMIT.ViewModels;

public class UserViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public int? EnseignantId { get; set; }
}

public class CreateUserViewModel
{
    [Required]
    public string Nom { get; set; } = string.Empty;

    [Required]
    public string Prenom { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
    public int? EnseignantId { get; set; }
}

public class EditRolesViewModel
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> CurrentRoles { get; set; } = new();
    public List<string> SelectedRoles { get; set; } = new();
}
