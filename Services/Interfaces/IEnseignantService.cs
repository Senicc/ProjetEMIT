using ProjetEMIT.Models;
using ProjetEMIT.ViewModels;

namespace ProjetEMIT.Services.Interfaces;

public interface IEnseignantService
{
    Task<IEnumerable<Enseignant>> GetAllAsync(string? search = null);
    Task<Enseignant?> GetWithMatieresAsync(int id);
    Task<IEnumerable<Seance>> GetSeancesAsync(int enseignantId);
    Task<bool> CreateAsync(Enseignant enseignant, IEnumerable<int> matiereIds);
    Task<bool> UpdateAsync(EnseignantViewModel model);
    Task<bool> DeleteAsync(int id);
    Task<int> GetTotalCountAsync();
    Task<int> GetTotalClassesAsync();
}
