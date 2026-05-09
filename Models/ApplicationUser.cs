// Models/ApplicationUser.cs
public class ApplicationUser : IdentityUser
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int? EnseignantId { get; set; }        // Lien avec Enseignant si rôle Enseignant
    public Enseignant? Enseignant { get; set; }
}