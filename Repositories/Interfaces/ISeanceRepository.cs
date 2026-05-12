using ProjetEMIT.Models;

namespace ProjetEMIT.Repositories.Interfaces;

public interface ISeanceRepository : IBaseRepository<Seance>
{
    Task<IEnumerable<Seance>> GetEmploiDuTempsParClasseAsync(int classeId, DateOnly? dateDebut = null, DateOnly? dateFin = null);
    Task<IEnumerable<Seance>> GetEmploiDuTempsParEnseignantAsync(int enseignantId, DateOnly? dateDebut = null, DateOnly? dateFin = null);
    Task<IEnumerable<Seance>> GetEmploiDuTempsParSalleAsync(int salleId, DateOnly? dateDebut = null, DateOnly? dateFin = null);
    Task<IEnumerable<Seance>> GetAllInRangeAsync(DateOnly? dateDebut = null, DateOnly? dateFin = null);
    Task<int> CountSeancesDuJourAsync(DateOnly jour);
    Task<IReadOnlyList<(string NomSalle, int Count)>> GetSeanceCountsBySalleAsync(int top);
    Task<IReadOnlyList<(DateOnly Jour, int Count)>> GetSeanceCountsByDayAsync(int nombreJours);

    Task<bool> SalleOccupeeParSeanceAsync(int salleId, DateOnly date, TimeOnly debut, TimeOnly fin);

    Task<bool> ExisteConflitSalleAsync(int salleId, DateOnly date, TimeOnly debut, TimeOnly fin, int? seanceIdExclure = null);
    Task<bool> ExisteConflitEnseignantAsync(int enseignantId, DateOnly date, TimeOnly debut, TimeOnly fin, int? seanceIdExclure = null);
    Task<bool> ExisteConflitClasseAsync(int classeId, DateOnly date, TimeOnly debut, TimeOnly fin, int? seanceIdExclure = null);
}
