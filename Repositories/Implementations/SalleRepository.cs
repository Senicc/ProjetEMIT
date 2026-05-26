using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Data;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Repositories.Implementations;

public class SalleRepository : BaseRepository<Salle>, ISalleRepository
{
    public SalleRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Salle>> GetSallesDisponiblesAsync(DateOnly date, TimeOnly heureDebut, TimeOnly heureFin)
    {
        var debut = heureDebut.ToTimeSpan();
        var fin   = heureFin.ToTimeSpan();

        return await _context.Salles
            .AsNoTracking()
            .Where(s => s.EstDisponible)
            .Where(s => !s.Seances.Any(se =>
                se.Date == date &&
                se.HeureDebut < fin &&
                se.HeureFin > debut))
            .ToListAsync();
    }

    public async Task<bool> EstDisponibleAsync(int salleId, DateOnly date, TimeOnly debut, TimeOnly fin)
    {
        var debutTs = debut.ToTimeSpan();
        var finTs   = fin.ToTimeSpan();

        return !await _context.Seances.AnyAsync(s =>
            s.SalleId == salleId &&
            s.Date == date &&
            s.HeureDebut < finTs &&
            s.HeureFin > debutTs);
    }

    public async Task<IEnumerable<Salle>> SearchAsync(string terme)
    {
        terme = terme.Trim();
        return await _context.Salles
            .AsNoTracking()
            .Where(s => s.NomSalle.Contains(terme) || s.Localisation.Contains(terme))
            .ToListAsync();
    }
}
