using System.ComponentModel.DataAnnotations;

namespace ProjetEMIT.ViewModels;

public class SeanceViewModel
{
    [Required]
    [DataType(DataType.Date)]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required]
    [Display(Name = "Créneau")]
    public int CreneauId { get; set; }

    [Required]
    [Display(Name = "Salle")]
    public int SalleId { get; set; }

    [Required]
    [Display(Name = "Enseignant")]
    public int EnseignantId { get; set; }

    [Required]
    [Display(Name = "Matière")]
    public int MatiereId { get; set; }

    [Required]
    [Display(Name = "Classe")]
    public int ClasseId { get; set; }

    [Required]
    [Display(Name = "Type de séance")]
    public string TypeSeance { get; set; } = "Cours";
}
