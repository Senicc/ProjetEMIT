public class Enseignant
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Specialite { get; set; } = string.Empty;

    public ICollection<Matiere> Matieres { get; set; } = new();
    public ICollection<Seance> Seances { get; set; } = new();
}