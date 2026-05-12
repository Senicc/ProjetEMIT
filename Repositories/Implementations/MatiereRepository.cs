using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Data;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Repositories.Implementations;

public class MatiereRepository : BaseRepository<Matiere>, IMatiereRepository
{
    public MatiereRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Matiere>> GetAllWithDetailsAsync(string? search = null)
    {
        IQueryable<Matiere> q = _context.Matieres.AsNoTracking().Include(m => m.Filiere).Include(m => m.Enseignants);
        if (!string.IsNullOrWhiteSpace(search))
        {
            var t = search.Trim();
            q = q.Where(m => m.Code.Contains(t) || m.Nom.Contains(t));
        }

        return await q.OrderBy(m => m.Code).ToListAsync();
    }

    public async Task<Matiere?> GetWithDetailsAsync(int id)
    {
        return await _context.Matieres
            .Include(m => m.Filiere)
            .Include(m => m.Enseignants)
            .Include(m => m.Seances)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task ReplaceEnseignantsAsync(int matiereId, IEnumerable<int> enseignantIds)
    {
        var entity = await _context.Matieres
            .Include(m => m.Enseignants)
            .FirstOrDefaultAsync(m => m.Id == matiereId)
            ?? throw new InvalidOperationException("Matière introuvable.");

        entity.Enseignants.Clear();
        var ids = enseignantIds.Distinct().ToList();
        if (ids.Count == 0)
        {
            await _context.SaveChangesAsync();
            return;
        }

        var enseignants = await _context.Enseignants.Where(e => ids.Contains(e.Id)).ToListAsync();
        foreach (var e in enseignants)
            entity.Enseignants.Add(e);

        await _context.SaveChangesAsync();
    }
}
