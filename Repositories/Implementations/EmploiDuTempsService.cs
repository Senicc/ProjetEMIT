// Services/Implementations/EmploiDuTempsService.cs
public class EmploiDuTempsService : IEmploiDuTempsService
{
    private readonly ISeanceRepository _seanceRepository;
    private readonly ISalleRepository _salleRepository;

    public EmploiDuTempsService(ISeanceRepository seanceRepository, ISalleRepository salleRepository)
    {
        _seanceRepository = seanceRepository;
        _salleRepository = salleRepository;
    }

    public async Task<ConflitResult> VerifierConflitsAsync(Seance seance, bool exclureSeanceActuelle = false)
    {
        var result = new ConflitResult();
        var exclureId = exclureSeanceActuelle ? seance.Id : null;

        // Conflit Salle
        bool conflitSalle = await _seanceRepository.ExisteConflitSalleAsync(
            seance.SalleId, seance.Date, seance.Creneau.HeureDebut, seance.Creneau.HeureFin, exclureId);

        if (conflitSalle)
        {
            result.Conflits.Add("Salle déjà occupée à cet horaire");
        }

        // Conflit Enseignant
        bool conflitEnseignant = await _seanceRepository.ExisteConflitEnseignantAsync(
            seance.EnseignantId, seance.Date, seance.Creneau.HeureDebut, seance.Creneau.HeureFin, exclureId);

        if (conflitEnseignant)
        {
            result.Conflits.Add("Enseignant déjà occupé à cet horaire");
        }

        // Conflit Classe
        bool conflitClasse = await _seanceRepository.ExisteConflitClasseAsync(
            seance.ClasseId, seance.Date, seance.Creneau.HeureDebut, seance.Creneau.HeureFin, exclureId);

        if (conflitClasse)
        {
            result.Conflits.Add("Classe déjà en cours à cet horaire");
        }

        result.HasConflit = result.Conflits.Any();
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

    public async Task<IEnumerable<Seance>> GetEmploiParClasseAsync(int classeId, DateOnly? debut = null, DateOnly? fin = null)
    {
        return await _seanceRepository.GetEmploiDuTempsParClasseAsync(classeId, debut, fin);
    }

    // Autres méthodes...
    public async Task<IEnumerable<Seance>> GetEmploiParEnseignantAsync(int enseignantId, DateOnly? debut = null, DateOnly? fin = null)
    {
        return await _seanceRepository.GetEmploiDuTempsParEnseignantAsync(enseignantId, debut, fin);
    }

    public Task<IEnumerable<Seance>> GetEmploiParSalleAsync(int salleId, DateOnly date)
    {
        return _seanceRepository.GetEmploiDuTempsParSalleAsync(salleId, date);
    }

    public Task<bool> ModifierSeanceAsync(Seance seance) => throw new NotImplementedException("À implémenter");
    public Task<bool> SupprimerSeanceAsync(int id) => throw new NotImplementedException("À implémenter");
}