using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;
using ProjetEMIT.Services.Interfaces;

namespace ProjetEMIT.Services.Implementations;

public class MatiereService : IMatiereService
{
    private readonly IMatiereRepository _matiereRepository;

    public MatiereService(IMatiereRepository matiereRepository)
    {
        _matiereRepository = matiereRepository;
    }

    public Task<IEnumerable<Matiere>> GetAllWithDetailsAsync(string? search) =>
        _matiereRepository.GetAllWithDetailsAsync(search);

    public async Task<IEnumerable<Matiere>> GetAllAsync()
    {
        return await _matiereRepository.GetAllWithDetailsAsync(null);
    }

    public async Task<Matiere?> GetByIdAsync(int id)
    {
        return await _matiereRepository.GetWithDetailsAsync(id);
    }

    public async Task<bool> CreateAsync(Matiere matiere, IEnumerable<int> enseignantIds)
    {
        await _matiereRepository.AddAsync(matiere);
        await _matiereRepository.ReplaceEnseignantsAsync(matiere.Id, enseignantIds);
        return true;
    }

    public async Task<bool> UpdateAsync(Matiere matiere, IEnumerable<int> enseignantIds)
    {
        await _matiereRepository.UpdateAsync(matiere);
        await _matiereRepository.ReplaceEnseignantsAsync(matiere.Id, enseignantIds);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var m = await _matiereRepository.GetWithDetailsAsync(id);
        if (m == null) return false;
        if (m.Seances.Any()) return false;
        await _matiereRepository.DeleteAsync(id);
        return true;
    }
}
