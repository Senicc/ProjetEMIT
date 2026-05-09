// Repositories/Interfaces/ISeanceRepository.cs
public interface ISeanceRepository : IBaseRepository<Seance>
{
    Task<IEnumerable<Seance>> GetEmploiDuTempsParClasseAsync(int classeId, DateOnly? dateDebut = null, DateOnly? dateFin = null);
    Task<IEnumerable<Seance>> GetEmploiDuTempsParEnseignantAsync(int enseignantId, DateOnly? dateDebut = null, DateOnly? dateFin = null);
    Task<IEnumerable<Seance>> GetEmploiDuTempsParSalleAsync(int salleId, DateOnly date);

    // Détection de conflits
    Task<bool> ExisteConflitSalleAsync(int salleId, DateOnly date, TimeOnly debut, TimeOnly fin, int? seanceIdExclure = null);
    Task<bool> ExisteConflitEnseignantAsync(int enseignantId, DateOnly date, TimeOnly debut, TimeOnly fin, int? seanceIdExclure = null);
    Task<bool> ExisteConflitClasseAsync(int classeId, DateOnly date, TimeOnly debut, TimeOnly fin, int? seanceIdExclure = null);
}