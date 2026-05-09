public class Reservation
{
    public int Id { get; set; }
    public int SalleId { get; set; }
    public Salle Salle { get; set; } = null!;

    public int DemandeurId { get; set; }           // Utilisateur (Enseignant ou Responsable)
    public ApplicationUser Demandeur { get; set; } = null!;

    public DateOnly Date { get; set; }
    public TimeOnly HeureDebut { get; set; }
    public TimeOnly HeureFin { get; set; }

    public string Motif { get; set; } = string.Empty;
    public string Statut { get; set; } = "EnAttente"; // EnAttente, Validee, Refusee, Annulee
    public DateTime DateDemande { get; set; } = DateTime.UtcNow;
}