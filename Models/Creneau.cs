public class Creneau
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;        // ex: Créneau 1
    public TimeOnly HeureDebut { get; set; }
    public TimeOnly HeureFin { get; set; }

    public ICollection<Seance> Seances { get; set; } = new();
}