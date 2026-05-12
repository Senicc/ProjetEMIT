using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Data;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;
using ProjetEMIT.ViewModels;

namespace ProjetEMIT.Repositories.Implementations;

public class EnseignantRepository : BaseRepository<Enseignant>, IEnseignantRepository
{
    public EnseignantRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Enseignant?> GetWithMatieresAsync(int id)
    {
        return await _context.Enseignants
            .Include(e => e.Matieres)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Enseignant>> GetBySpecialiteAsync(string specialite)
    {
        return await _context.Enseignants
            .AsNoTracking()
            .Where(e => e.Specialite.Contains(specialite))
            .ToListAsync();
    }

    public async Task<IEnumerable<Enseignant>> SearchAsync(string? term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return await _context.Enseignants.AsNoTracking().Include(e => e.Matieres).ToListAsync();

        term = term.Trim();
        return await _context.Enseignants
            .AsNoTracking()
            .Include(e => e.Matieres)
            .Where(e => e.Nom.Contains(term) || e.Prenom.Contains(term) || e.Email.Contains(term))
            .ToListAsync();
    }

    public async Task ReplaceMatieresAsync(int enseignantId, IEnumerable<int> matiereIds)
    {
        var entity = await _context.Enseignants
            .Include(e => e.Matieres)
            .FirstOrDefaultAsync(e => e.Id == enseignantId)
            ?? throw new InvalidOperationException("Enseignant introuvable.");

        entity.Matieres.Clear();
        var ids = matiereIds.Distinct().ToList();
        if (ids.Count == 0)
        {
            await _context.SaveChangesAsync();
            return;
        }

        var matieres = await _context.Matieres.Where(m => ids.Contains(m.Id)).ToListAsync();
        foreach (var m in matieres)
            entity.Matieres.Add(m);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateFromViewModelAsync(EnseignantViewModel model)
    {
        var entity = await _context.Enseignants
            .Include(e => e.Matieres)
            .FirstOrDefaultAsync(e => e.Id == model.Id);

        if (entity == null) return false;

        entity.Nom = model.Nom;
        entity.Prenom = model.Prenom;
        entity.Email = model.Email;
        entity.Telephone = model.Telephone;
        entity.Specialite = model.Specialite;

        entity.Matieres.Clear();
        var ids = model.SelectedMatieresIds.Distinct().ToList();
        if (ids.Count > 0)
        {
            var matieres = await _context.Matieres.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var m in matieres)
                entity.Matieres.Add(m);
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
