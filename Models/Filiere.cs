public class Filiere
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty; // ex: INFO, GEST
    public string Nom { get; set; } = string.Empty;   // Informatique, Gestion, etc.

    public ICollection<Matiere> Matieres { get; set; } = new();
    public ICollection<Classe> Classes { get; set; } = new();
}