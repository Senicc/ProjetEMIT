using System.ComponentModel.DataAnnotations;

namespace ProjetEMIT.ViewModels;

public class ReservationViewModel
{
    public int SalleId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly Date { get; set; }

    [Required]
    public TimeOnly HeureDebut { get; set; }

    [Required]
    public TimeOnly HeureFin { get; set; }

    [Required]
    public string Motif { get; set; } = string.Empty;
}
