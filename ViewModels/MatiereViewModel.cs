using System.ComponentModel.DataAnnotations;

namespace ProjetEMIT.ViewModels;

public class MatiereViewModel
{
    public int Id { get; set; }

    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public string Nom { get; set; } = string.Empty;

    [Required]
    public int Coefficient { get; set; }

    [Required]
    public int NbreHeures { get; set; }

    [Required]
    public int FiliereId { get; set; }

    public List<int> SelectedEnseignantsIds { get; set; } = new();
}
