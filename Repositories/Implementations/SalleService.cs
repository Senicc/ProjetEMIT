// Services/Implementations/SalleService.cs
public class SalleService : ISalleService
{
    private readonly ISalleRepository _salleRepository;

    public SalleService(ISalleRepository salleRepository)
    {
        _salleRepository = salleRepository;
    }

    public async Task<IEnumerable<Salle>> GetAllSallesAsync() => await _salleRepository.GetAllAsync();

    public async Task<bool> CreateAsync(Salle salle)
    {
        await _salleRepository.AddAsync(salle);
        return true;
    }

    public async Task<Dictionary<string, int>> GetStatistiquesOccupationAsync()
    {
        var salles = await _salleRepository.GetAllAsync();
        // Logique de statistiques à enrichir plus tard
        return new Dictionary<string, int>
        {
            { "TotalSalles", salles.Count() },
            { "SallesDisponibles", salles.Count(s => s.EstDisponible) }
        };
    }
}