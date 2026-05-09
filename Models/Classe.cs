public class Classe
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty; // ex: L2 INFO A

    public int FiliereId { get; set; }
    public Filiere Filiere { get; set; } = null!;

    public int NiveauId { get; set; }
    public Niveau Niveau { get; set; } = null!;

    public ICollection<Seance> Seances { get; set; } = new List<Seance>();
}