using System.ComponentModel.DataAnnotations;

namespace ProjetEMIT.ViewModels;

public class SeanceViewModel
{
    [Required]
    [DataType(DataType.Date)]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required]
    [Display(Name = "Heure de d&eacute;but")]
    public TimeSpan HeureDebut { get; set; }

    [Required]
    [Display(Name = "Heure de fin")]
    public TimeSpan HeureFin { get; set; }

    [Required]
    [Display(Name = "Salle")]
    public int SalleId { get; set; }

    [Required]
    [Display(Name = "Enseignant")]
    public int EnseignantId { get; set; }

    [Required]
    [Display(Name = "Mati&egrave;re")]
    public int MatiereId { get; set; }

    [Required]
    [Display(Name = "Classe")]
    public int ClasseId { get; set; }

    [Required]
    [Display(Name = "Type de s&eacute;ance")]
    public string TypeSeance { get; set; } = "Cours";
}
