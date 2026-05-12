using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Repositories.Implementations;

public class SalleService : ISalleService
{
    private readonly ISalleRepository _salleRepository;
    private readonly ISeanceRepository _seanceRepository;
    private readonly IReservationRepository _reservationRepository;

    public SalleService(
        ISalleRepository salleRepository,
        ISeanceRepository seanceRepository,
        IReservationRepository reservationRepository)
    {
        _salleRepository = salleRepository;
        _seanceRepository = seanceRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<IEnumerable<Salle>> GetAllSallesAsync() =>
        await _salleRepository.GetAllAsync();

    public async Task<Salle?> GetByIdAsync(int id) =>
        await _salleRepository.GetByIdAsync(id);

    public async Task<bool> CreateAsync(Salle salle)
    {
        await _salleRepository.AddAsync(salle);
        return true;
    }

    public async Task<bool> UpdateAsync(Salle salle)
    {
        await _salleRepository.UpdateAsync(salle);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var salle = await _salleRepository.GetByIdAsync(id);
        if (salle == null) return false;

        if (await _seanceRepository.GetQueryable().AnyAsync(s => s.SalleId == id))
            return false;

        if (await _reservationRepository.ExistsActiveForSalleAsync(id))
            return false;

        await _salleRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<Salle>> GetSallesDisponiblesAsync(DateOnly date, TimeOnly debut, TimeOnly fin) =>
        await _salleRepository.GetSallesDisponiblesAsync(date, debut, fin);

    public async Task<IEnumerable<Salle>> SearchAsync(string terme)
    {
        if (string.IsNullOrWhiteSpace(terme))
            return await _salleRepository.GetAllAsync();
        return await _salleRepository.SearchAsync(terme);
    }

    public async Task<Dictionary<string, int>> GetStatistiquesOccupationAsync()
    {
        var salles = await _salleRepository.GetAllAsync();
        return new Dictionary<string, int>
        {
            ["TotalSalles"] = salles.Count(),
            ["SallesDisponibles"] = salles.Count(s => s.EstDisponible)
        };
    }

    public async Task<double> GetTauxOccupationAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var seances = await _seanceRepository.CountSeancesDuJourAsync(today);
        var salles = (await _salleRepository.GetAllAsync()).Count();
        if (salles == 0) return 0;
        var plafond = salles * 8d;
        return Math.Min(100, seances * 100.0 / plafond);
    }

    public async Task<List<SalleStatDto>> GetSallesLesPlusUtiliseesAsync(int top)
    {
        var rows = await _seanceRepository.GetSeanceCountsBySalleAsync(top);
        return rows.Select(r => new SalleStatDto { NomSalle = r.NomSalle, NombreSeances = r.Count }).ToList();
    }

    public async Task<(IReadOnlyList<string> Labels, IReadOnlyList<double> Values)> GetOccupationChartSeriesAsync(int nombreJours)
    {
        var data = await _seanceRepository.GetSeanceCountsByDayAsync(nombreJours);
        var salles = (await _salleRepository.GetAllAsync()).Count();
        var labels = data.Select(d => d.Jour.ToString("dd/MM")).ToList();
        var values = data.Select(d =>
        {
            if (salles == 0) return 0d;
            var plafond = salles * 8d;
            return Math.Round(Math.Min(100, d.Count * 100.0 / plafond), 1);
        }).ToList();
        return (labels, values);
    }
}
