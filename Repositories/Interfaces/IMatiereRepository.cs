using ProjetEMIT.Models;

namespace ProjetEMIT.Repositories.Interfaces;

public interface IMatiereRepository : IBaseRepository<Matiere>
{
    Task<IEnumerable<Matiere>> GetAllWithDetailsAsync(string? search = null);
    Task<Matiere?> GetWithDetailsAsync(int id);
    Task ReplaceEnseignantsAsync(int matiereId, IEnumerable<int> enseignantIds);
}
