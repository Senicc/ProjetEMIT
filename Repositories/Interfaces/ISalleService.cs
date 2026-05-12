using ProjetEMIT.Models;

namespace ProjetEMIT.Repositories.Interfaces;

public interface ISalleService
{
    Task<IEnumerable<Salle>> GetAllSallesAsync();
    Task<Salle?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Salle salle);
    Task<bool> UpdateAsync(Salle salle);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Salle>> GetSallesDisponiblesAsync(DateOnly date, TimeOnly debut, TimeOnly fin);
    Task<IEnumerable<Salle>> SearchAsync(string terme);
    Task<Dictionary<string, int>> GetStatistiquesOccupationAsync();
    Task<double> GetTauxOccupationAsync();
    Task<List<SalleStatDto>> GetSallesLesPlusUtiliseesAsync(int top);
    Task<(IReadOnlyList<string> Labels, IReadOnlyList<double> Values)> GetOccupationChartSeriesAsync(int nombreJours);
}

public class SalleStatDto
{
    public string NomSalle { get; set; } = string.Empty;
    public int NombreSeances { get; set; }
}
