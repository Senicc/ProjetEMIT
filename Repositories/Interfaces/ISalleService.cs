// Services/Interfaces/ISalleService.cs
public interface ISalleService
{
    Task<IEnumerable<Salle>> GetAllSallesAsync();
    Task<Salle?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Salle salle);
    Task<bool> UpdateAsync(Salle salle);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Salle>> GetSallesDisponiblesAsync(DateOnly date, TimeOnly debut, TimeOnly fin);
    Task<Dictionary<string, int>> GetStatistiquesOccupationAsync();
}