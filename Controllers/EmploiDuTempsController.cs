using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.ViewModels;

namespace ProjetEMIT.Controllers;

[Authorize]
public class EmploiDuTempsController : Controller
{
    private readonly IEmploiDuTempsService _emploiService;
    private readonly ISalleService _salleService;
    private readonly IEnseignantService _enseignantService;
    private readonly IRapportService _rapportService;
    private readonly ICreneauRepository _creneauRepository;
    private readonly IMatiereRepository _matiereRepository;
    private readonly IClasseRepository _classeRepository;

    public EmploiDuTempsController(
        IEmploiDuTempsService emploiService,
        ISalleService salleService,
        IEnseignantService enseignantService,
        IRapportService rapportService,
        ICreneauRepository creneauRepository,
        IMatiereRepository matiereRepository,
        IClasseRepository classeRepository)
    {
        _emploiService = emploiService;
        _salleService = salleService;
        _enseignantService = enseignantService;
        _rapportService = rapportService;
        _creneauRepository = creneauRepository;
        _matiereRepository = matiereRepository;
        _classeRepository = classeRepository;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<JsonResult> GetEvents(DateTime start, DateTime end,
        int? classeId = null, int? salleId = null, int? enseignantId = null)
    {
        var dateDebut = DateOnly.FromDateTime(start);
        var dateFin = DateOnly.FromDateTime(end);

        IEnumerable<Seance> seances;

        if (classeId.HasValue)
            seances = await _emploiService.GetEmploiParClasseAsync(classeId.Value, dateDebut, dateFin);
        else if (salleId.HasValue)
            seances = await _emploiService.GetEmploiParSalleAsync(salleId.Value, dateDebut, dateFin);
        else if (enseignantId.HasValue)
            seances = await _emploiService.GetEmploiParEnseignantAsync(enseignantId.Value, dateDebut, dateFin);
        else
            seances = await _emploiService.GetAllSeancesAsync(dateDebut, dateFin);

        var inv = CultureInfo.InvariantCulture;
        var events = seances.Select(s => new
        {
            id = s.Id,
            title = $"{s.Matiere.Code} - {s.Matiere.Nom}\n{s.Classe.Nom}",
            start = $"{s.Date.ToString("yyyy-MM-dd", inv)}T{s.Creneau.HeureDebut.ToString("HH\\:mm\\:ss", inv)}",
            end = $"{s.Date.ToString("yyyy-MM-dd", inv)}T{s.Creneau.HeureFin.ToString("HH\\:mm\\:ss", inv)}",
            color = GetEventColor(s.TypeSeance),
            extendedProps = new
            {
                salle = s.Salle.NomSalle,
                enseignant = $"{s.Enseignant.Prenom} {s.Enseignant.Nom}",
                classe = s.Classe.Nom,
                typeSeance = s.TypeSeance,
                matiere = s.Matiere.Nom
            }
        });

        return Json(events);
    }

    private static string GetEventColor(string typeSeance) =>
        typeSeance switch
        {
            "Cours" => "#1e88e5",
            "TD" => "#43a047",
            "TP" => "#f57c00",
            "Examen" => "#e53935",
            _ => "#6c757d"
        };

    [HttpGet]
    [Authorize(Roles = "Administrateur,ResponsablePedagogique")]
    public async Task<IActionResult> Create()
    {
        await PopulateSeanceLookupsAsync();
        return View(new SeanceViewModel());
    }

    [HttpGet]
    public async Task<IActionResult> ExportPdf(int? classeId = null, int? enseignantId = null, int? salleId = null)
    {
        var pdfBytes = await _rapportService.GenerateEmploiDuTempsPdfAsync(classeId, enseignantId, salleId);

        var fileName = classeId.HasValue ? "Emploi_Classe.pdf" :
            enseignantId.HasValue ? "Emploi_Enseignant.pdf" :
            salleId.HasValue ? "Emploi_Salle.pdf" : "Emploi_Du_Temps.pdf";

        return File(pdfBytes, "application/pdf", fileName);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrateur,ResponsablePedagogique")]
    public async Task<IActionResult> Create(SeanceViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateSeanceLookupsAsync();
            return View(model);
        }

        var seance = new Seance
        {
            Date = model.Date,
            CreneauId = model.CreneauId,
            SalleId = model.SalleId,
            EnseignantId = model.EnseignantId,
            MatiereId = model.MatiereId,
            ClasseId = model.ClasseId,
            TypeSeance = model.TypeSeance
        };

        try
        {
            await _emploiService.CreerSeanceAsync(seance);
            TempData["Success"] = "Séance ajoutée avec succès !";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            await PopulateSeanceLookupsAsync();
            return View(model);
        }
    }

    private async Task PopulateSeanceLookupsAsync()
    {
        ViewBag.Salles = await _salleService.GetAllSallesAsync();
        ViewBag.Enseignants = await _enseignantService.GetAllAsync();
        ViewBag.Creneaux = await _creneauRepository.GetAllAsync();
        ViewBag.Matieres = await _matiereRepository.GetAllWithDetailsAsync(null);
        ViewBag.Classes = await _classeRepository.GetAllWithDetailsAsync();
    }
}
