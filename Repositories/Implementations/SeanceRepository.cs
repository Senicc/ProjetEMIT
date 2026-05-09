// Repositories/Implementations/SeanceRepository.cs
public class SeanceRepository : BaseRepository<Seance>, ISeanceRepository
{
    public SeanceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<bool> ExisteConflitSalleAsync(int salleId, DateOnly date, TimeOnly debut, TimeOnly fin, int? seanceIdExclure = null)
    {
        return await _context.Seances.AnyAsync(s =>
            s.SalleId == salleId &&
            s.Date == date &&
            s.Creneau.HeureDebut < fin &&
            s.Creneau.HeureFin > debut &&
            (seanceIdExclure == null || s.Id != seanceIdExclure));
    }

    // Même logique pour Enseignant et Classe...
    public async Task<bool> ExisteConflitEnseignantAsync(int enseignantId, DateOnly date, TimeOnly debut, TimeOnly fin, int? seanceIdExclure = null)
    {
        return await _context.Seances.AnyAsync(s =>
            s.EnseignantId == enseignantId &&
            s.Date == date &&
            s.Creneau.HeureDebut < fin &&
            s.Creneau.HeureFin > debut &&
            (seanceIdExclure == null || s.Id != seanceIdExclure));
    }

    public async Task<bool> ExisteConflitClasseAsync(int classeId, DateOnly date, TimeOnly debut, TimeOnly fin, int? seanceIdExclure = null)
    {
        return await _context.Seances.AnyAsync(s =>
            s.ClasseId == classeId &&
            s.Date == date &&
            s.Creneau.HeureDebut < fin &&
            s.Creneau.HeureFin > debut &&
            (seanceIdExclure == null || s.Id != seanceIdExclure));
    }

    public async Task<IEnumerable<Seance>> GetEmploiDuTempsParClasseAsync(int classeId, DateOnly? dateDebut = null, DateOnly? dateFin = null)
    {
        var query = _context.Seances
            .Include(s => s.Matiere)
            .Include(s => s.Enseignant)
            .Include(s => s.Salle)
            .Include(s => s.Creneau)
            .Where(s => s.ClasseId == classeId);

        if (dateDebut.HasValue) query = query.Where(s => s.Date >= dateDebut.Value);
        if (dateFin.HasValue) query = query.Where(s => s.Date <= dateFin.Value);

        return await query.OrderBy(s => s.Date).ThenBy(s => s.Creneau.HeureDebut).ToListAsync();
    }
}