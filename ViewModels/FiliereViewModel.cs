using System.ComponentModel.DataAnnotations;

namespace ProjetEMIT.ViewModels;

public class FiliereViewModel
{
    public int Id { get; set; }

    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public string Nom { get; set; } = string.Empty;
}
