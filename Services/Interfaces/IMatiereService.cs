using ProjetEMIT.Models;

namespace ProjetEMIT.Services.Interfaces;

public interface IMatiereService
{
    Task<IEnumerable<Matiere>> GetAllWithDetailsAsync(string? search);
    Task<IEnumerable<Matiere>> GetAllAsync();
    Task<Matiere?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Matiere matiere, IEnumerable<int> enseignantIds);
    Task<bool> UpdateAsync(Matiere matiere, IEnumerable<int> enseignantIds);
    Task<bool> DeleteAsync(int id);
}
