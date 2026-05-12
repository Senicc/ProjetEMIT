using System.ComponentModel.DataAnnotations;

namespace ProjetEMIT.ViewModels;

public class ClasseViewModel
{
    public int Id { get; set; }

    [Required]
    public string Nom { get; set; } = string.Empty;

    [Required]
    public int FiliereId { get; set; }

    [Required]
    public int NiveauId { get; set; }
}
