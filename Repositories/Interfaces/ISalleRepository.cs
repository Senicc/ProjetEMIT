using ProjetEMIT.Models;

namespace ProjetEMIT.Repositories.Interfaces;

public interface ISalleRepository : IBaseRepository<Salle>
{
    Task<IEnumerable<Salle>> GetSallesDisponiblesAsync(DateOnly date, TimeOnly heureDebut, TimeOnly heureFin);
    Task<bool> EstDisponibleAsync(int salleId, DateOnly date, TimeOnly debut, TimeOnly fin);
    Task<IEnumerable<Salle>> SearchAsync(string terme);
}
