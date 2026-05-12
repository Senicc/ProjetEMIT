using ProjetEMIT.Models;
using ProjetEMIT.ViewModels;

namespace ProjetEMIT.Repositories.Interfaces;

public interface IEnseignantRepository : IBaseRepository<Enseignant>
{
    Task<Enseignant?> GetWithMatieresAsync(int id);
    Task<IEnumerable<Enseignant>> GetBySpecialiteAsync(string specialite);
    Task<IEnumerable<Enseignant>> SearchAsync(string? term);
    Task ReplaceMatieresAsync(int enseignantId, IEnumerable<int> matiereIds);
    Task<bool> UpdateFromViewModelAsync(EnseignantViewModel model);
}
