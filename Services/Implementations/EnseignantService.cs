using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.ViewModels;

namespace ProjetEMIT.Services.Implementations;

public class EnseignantService : IEnseignantService
{
    private readonly IEnseignantRepository _enseignantRepository;
    private readonly ISeanceRepository _seanceRepository;
    private readonly IClasseRepository _classeRepository;

    public EnseignantService(
        IEnseignantRepository enseignantRepository,
        ISeanceRepository seanceRepository,
        IClasseRepository classeRepository)
    {
        _enseignantRepository = enseignantRepository;
        _seanceRepository = seanceRepository;
        _classeRepository = classeRepository;
    }

    public async Task<IEnumerable<Enseignant>> GetAllAsync(string? search = null) =>
        await _enseignantRepository.SearchAsync(search);

    public Task<Enseignant?> GetWithMatieresAsync(int id) =>
        _enseignantRepository.GetWithMatieresAsync(id);

    public async Task<IEnumerable<Seance>> GetSeancesAsync(int enseignantId) =>
        await _seanceRepository.GetEmploiDuTempsParEnseignantAsync(enseignantId, null, null);

    public async Task<bool> CreateAsync(Enseignant enseignant, IEnumerable<int> matiereIds)
    {
        await _enseignantRepository.AddAsync(enseignant);
        await _enseignantRepository.ReplaceMatieresAsync(enseignant.Id, matiereIds);
        return true;
    }

    public Task<bool> UpdateAsync(EnseignantViewModel model) =>
        _enseignantRepository.UpdateFromViewModelAsync(model);

    public async Task<bool> DeleteAsync(int id)
    {
        var seances = await _seanceRepository.GetEmploiDuTempsParEnseignantAsync(id, null, null);
        if (seances.Any())
            return false;

        await _enseignantRepository.DeleteAsync(id);
        return true;
    }

    public async Task<int> GetTotalCountAsync()
    {
        var all = await _enseignantRepository.GetAllAsync();
        return all.Count();
    }

    public async Task<int> GetTotalClassesAsync()
    {
        var all = await _classeRepository.GetAllAsync();
        return all.Count();
    }
}
