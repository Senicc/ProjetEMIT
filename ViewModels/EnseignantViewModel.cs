using System.ComponentModel.DataAnnotations;

namespace ProjetEMIT.ViewModels;

public class EnseignantViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Nom")]
    public string Nom { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Prénom")]
    public string Prenom { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string Telephone { get; set; } = string.Empty;

    public string Specialite { get; set; } = string.Empty;

    public List<int> SelectedMatieresIds { get; set; } = new();
}
