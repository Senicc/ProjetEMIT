using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Data;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Repositories.Implementations;

public class SeanceRepository : BaseRepository<Seance>, ISeanceRepository
{
    public SeanceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<bool> SalleOccupeeParSeanceAsync(int salleId, DateOnly date, TimeOnly debut, TimeOnly fin)
    {
        return await _context.Seances.AnyAsync(s =>
            s.SalleId == salleId &&
            s.Date == date &&
            s.HeureDebut < fin.ToTimeSpan() &&
            s.HeureFin > debut.ToTimeSpan());
    }

    public async Task<bool> ExisteConflitSalleAsync(int salleId, DateOnly date, TimeOnly debut, TimeOnly fin, int? seanceIdExclure = null)
    {
        return await _context.Seances.AnyAsync(s =>
            s.SalleId == salleId &&
            s.Date == date &&
            s.HeureDebut < fin.ToTimeSpan() &&
            s.HeureFin > debut.ToTimeSpan() &&
            (seanceIdExclure == null || s.Id != seanceIdExclure));
    }

    public async Task<bool> ExisteConflitEnseignantAsync(int enseignantId, DateOnly date, TimeOnly debut, TimeOnly fin, int? seanceIdExclure = null)
    {
        return await _context.Seances.AnyAsync(s =>
            s.EnseignantId == enseignantId &&
            s.Date == date &&
            s.HeureDebut < fin.ToTimeSpan() &&
            s.HeureFin > debut.ToTimeSpan() &&
            (seanceIdExclure == null || s.Id != seanceIdExclure));
    }

    public async Task<bool> ExisteConflitClasseAsync(int classeId, DateOnly date, TimeOnly debut, TimeOnly fin, int? seanceIdExclure = null)
    {
        return await _context.Seances.AnyAsync(s =>
            s.ClasseId == classeId &&
            s.Date == date &&
            s.HeureDebut < fin.ToTimeSpan() &&
            s.HeureFin > debut.ToTimeSpan() &&
            (seanceIdExclure == null || s.Id != seanceIdExclure));
    }

    public async Task<IEnumerable<Seance>> GetEmploiDuTempsParClasseAsync(int classeId, DateOnly? dateDebut = null, DateOnly? dateFin = null)
    {
        var query = BaseQuery().Where(s => s.ClasseId == classeId);
        if (dateDebut.HasValue) query = query.Where(s => s.Date >= dateDebut.Value);
        if (dateFin.HasValue) query = query.Where(s => s.Date <= dateFin.Value);
        return await query.OrderBy(s => s.Date).ThenBy(s => s.HeureDebut).ToListAsync();
    }

    public async Task<IEnumerable<Seance>> GetEmploiDuTempsParEnseignantAsync(int enseignantId, DateOnly? dateDebut = null, DateOnly? dateFin = null)
    {
        var query = BaseQuery().Where(s => s.EnseignantId == enseignantId);
        if (dateDebut.HasValue) query = query.Where(s => s.Date >= dateDebut.Value);
        if (dateFin.HasValue) query = query.Where(s => s.Date <= dateFin.Value);
        return await query.OrderBy(s => s.Date).ThenBy(s => s.HeureDebut).ToListAsync();
    }

    public async Task<IEnumerable<Seance>> GetEmploiDuTempsParSalleAsync(int salleId, DateOnly? dateDebut = null, DateOnly? dateFin = null)
    {
        var query = BaseQuery().Where(s => s.SalleId == salleId);
        if (dateDebut.HasValue) query = query.Where(s => s.Date >= dateDebut.Value);
        if (dateFin.HasValue) query = query.Where(s => s.Date <= dateFin.Value);
        return await query.OrderBy(s => s.Date).ThenBy(s => s.HeureDebut).ToListAsync();
    }

    public async Task<IEnumerable<Seance>> GetAllInRangeAsync(DateOnly? dateDebut = null, DateOnly? dateFin = null)
    {
        var query = BaseQuery();
        if (dateDebut.HasValue) query = query.Where(s => s.Date >= dateDebut.Value);
        if (dateFin.HasValue) query = query.Where(s => s.Date <= dateFin.Value);
        return await query.OrderBy(s => s.Date).ThenBy(s => s.HeureDebut).ToListAsync();
    }

    public async Task<int> CountSeancesDuJourAsync(DateOnly jour) =>
        await _context.Seances.CountAsync(s => s.Date == jour);

    public async Task<IReadOnlyList<(string NomSalle, int Count)>> GetSeanceCountsBySalleAsync(int top)
    {
        var grouped = await _context.Seances
            .GroupBy(s => s.SalleId)
            .Select(g => new { SalleId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(top)
            .ToListAsync();

        if (grouped.Count == 0)
            return Array.Empty<(string NomSalle, int Count)>();

        var ids = grouped.Select(x => x.SalleId).ToList();
        var noms = await _context.Salles.AsNoTracking()
            .Where(s => ids.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => s.NomSalle);

        return grouped
            .Select(g => (noms.GetValueOrDefault(g.SalleId, "?"), g.Count))
            .ToList();
    }

    public async Task<IReadOnlyList<(DateOnly Jour, int Count)>> GetSeanceCountsByDayAsync(int nombreJours)
    {
        var fin = DateOnly.FromDateTime(DateTime.Today);
        var debut = fin.AddDays(-(nombreJours - 1));
        var rows = await _context.Seances
            .Where(s => s.Date >= debut && s.Date <= fin)
            .GroupBy(s => s.Date)
            .Select(g => new { Jour = g.Key, Count = g.Count() })
            .ToListAsync();

        var dict = rows.ToDictionary(x => x.Jour, x => x.Count);
        var result = new List<(DateOnly Jour, int Count)>();
        for (var d = debut; d <= fin; d = d.AddDays(1))
            result.Add((d, dict.TryGetValue(d, out var c) ? c : 0));
        return result;
    }

    private IQueryable<Seance> BaseQuery() =>
        _context.Seances
            .AsNoTracking()
            .Include(s => s.Matiere)
            .Include(s => s.Enseignant)
            .Include(s => s.Salle)
            .Include(s => s.Classe);
}
