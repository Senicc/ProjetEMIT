public class Niveau
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty; // L1, L2, L3, M1, M2

    public ICollection<Classe> Classes { get; set; } = new();
}