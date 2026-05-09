// Services/Interfaces/IEmploiDuTempsService.cs
public interface IEmploiDuTempsService
{
    Task<bool> CreerSeanceAsync(Seance seance);
    Task<bool> ModifierSeanceAsync(Seance seance);
    Task<bool> SupprimerSeanceAsync(int id);

    Task<IEnumerable<Seance>> GetEmploiParClasseAsync(int classeId, DateOnly? debut = null, DateOnly? fin = null);
    Task<IEnumerable<Seance>> GetEmploiParEnseignantAsync(int enseignantId, DateOnly? debut = null, DateOnly? fin = null);
    Task<IEnumerable<Seance>> GetEmploiParSalleAsync(int salleId, DateOnly date);

    Task<ConflitResult> VerifierConflitsAsync(Seance seance, bool exclureSeanceActuelle = false);
}

public class ConflitResult
{
    public bool HasConflit { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Conflits { get; set; } = new();
}