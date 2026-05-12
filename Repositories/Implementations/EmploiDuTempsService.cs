using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Data;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Repositories.Implementations;

public class EmploiDuTempsService : IEmploiDuTempsService
{
    private readonly ISeanceRepository _seanceRepository;
    private readonly ApplicationDbContext _context;

    public EmploiDuTempsService(ISeanceRepository seanceRepository, ApplicationDbContext context)
    {
        _seanceRepository = seanceRepository;
        _context = context;
    }

    private async Task<Creneau> ResolveCreneauAsync(Seance seance)
    {
        if (seance.Creneau != null) return seance.Creneau;
        var c = await _context.Creneaux.AsNoTracking().FirstOrDefaultAsync(x => x.Id == seance.CreneauId);
        return c ?? throw new InvalidOperationException("Créneau introuvable.");
    }

    public async Task<ConflitResult> VerifierConflitsAsync(Seance seance, bool exclureSeanceActuelle = false)
    {
        var result = new ConflitResult();
        var exclureId = exclureSeanceActuelle ? seance.Id : (int?)null;
        var creneau = await ResolveCreneauAsync(seance);

        if (await _seanceRepository.ExisteConflitSalleAsync(
                seance.SalleId, seance.Date, creneau.HeureDebut, creneau.HeureFin, exclureId))
            result.Conflits.Add("Salle déjà occupée à cet horaire");

        if (await _seanceRepository.ExisteConflitEnseignantAsync(
                seance.EnseignantId, seance.Date, creneau.HeureDebut, creneau.HeureFin, exclureId))
            result.Conflits.Add("Enseignant déjà occupé à cet horaire");

        if (await _seanceRepository.ExisteConflitClasseAsync(
                seance.ClasseId, seance.Date, creneau.HeureDebut, creneau.HeureFin, exclureId))
            result.Conflits.Add("Classe déjà en cours à cet horaire");

        result.HasConflit = result.Conflits.Count > 0;
        result.Message = result.HasConflit
            ? "Conflits détectés : " + string.Join(" | ", result.Conflits)
            : "Aucun conflit détecté";

        return result;
    }

    public async Task<bool> CreerSeanceAsync(Seance seance)
    {
        var conflits = await VerifierConflitsAsync(seance);
        if (conflits.HasConflit)
            throw new InvalidOperationException(conflits.Message);

        await _seanceRepository.AddAsync(seance);
        return true;
    }

    public async Task<bool> ModifierSeanceAsync(Seance seance)
    {
        var conflits = await VerifierConflitsAsync(seance, exclureSeanceActuelle: true);
        if (conflits.HasConflit)
            throw new InvalidOperationException(conflits.Message);

        await _seanceRepository.UpdateAsync(seance);
        return true;
    }

    public async Task<bool> SupprimerSeanceAsync(int id)
    {
        await _seanceRepository.DeleteAsync(id);
        return true;
    }

    public Task<IEnumerable<Seance>> GetEmploiParClasseAsync(int classeId, DateOnly? debut = null, DateOnly? fin = null) =>
        _seanceRepository.GetEmploiDuTempsParClasseAsync(classeId, debut, fin);

    public Task<IEnumerable<Seance>> GetEmploiParEnseignantAsync(int enseignantId, DateOnly? debut = null, DateOnly? fin = null) =>
        _seanceRepository.GetEmploiDuTempsParEnseignantAsync(enseignantId, debut, fin);

    public Task<IEnumerable<Seance>> GetEmploiParSalleAsync(int salleId, DateOnly? debut = null, DateOnly? fin = null) =>
        _seanceRepository.GetEmploiDuTempsParSalleAsync(salleId, debut, fin);

    public Task<IEnumerable<Seance>> GetAllSeancesAsync(DateOnly? debut = null, DateOnly? fin = null) =>
        _seanceRepository.GetAllInRangeAsync(debut, fin);

    public async Task<int> GetSeancesDuJourCountAsync(DateOnly? jour = null)
    {
        var d = jour ?? DateOnly.FromDateTime(DateTime.Today);
        return await _seanceRepository.CountSeancesDuJourAsync(d);
    }
}
