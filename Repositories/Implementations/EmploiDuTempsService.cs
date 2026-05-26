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

    public async Task<ConflitResult> VerifierConflitsAsync(Seance seance, bool exclureSeanceActuelle = false)
    {
        var result = new ConflitResult();
        var exclureId = exclureSeanceActuelle ? seance.Id : (int?)null;

        var debut = TimeOnly.FromTimeSpan(seance.HeureDebut);
        var fin   = TimeOnly.FromTimeSpan(seance.HeureFin);

        if (await _seanceRepository.ExisteConflitSalleAsync(
                seance.SalleId, seance.Date, debut, fin, exclureId))
            result.Conflits.Add("Salle d&eacute;j&agrave; occup&eacute;e &agrave; cet horaire");

        if (await _seanceRepository.ExisteConflitEnseignantAsync(
                seance.EnseignantId, seance.Date, debut, fin, exclureId))
            result.Conflits.Add("Enseignant d&eacute;j&agrave; occup&eacute; &agrave; cet horaire");

        if (await _seanceRepository.ExisteConflitClasseAsync(
                seance.ClasseId, seance.Date, debut, fin, exclureId))
            result.Conflits.Add("Classe d&eacute;j&agrave; en cours &agrave; cet horaire");

        result.HasConflit = result.Conflits.Count > 0;
        result.Message = result.HasConflit
            ? "Conflits d&eacute;tect&eacute;s : " + string.Join(" | ", result.Conflits)
            : "Aucun conflit d&eacute;tect&eacute;";

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
