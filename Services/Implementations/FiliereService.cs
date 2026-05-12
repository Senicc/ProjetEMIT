using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;
using ProjetEMIT.Services.Interfaces;

namespace ProjetEMIT.Services.Implementations;

public class FiliereService : IFiliereService
{
    private readonly IFiliereRepository _filiereRepository;

    public FiliereService(IFiliereRepository filiereRepository)
    {
        _filiereRepository = filiereRepository;
    }

    public async Task<IEnumerable<Filiere>> GetAllAsync() =>
        await _filiereRepository.GetAllAsync();
}
