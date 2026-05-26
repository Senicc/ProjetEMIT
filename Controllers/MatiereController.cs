using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetEMIT.Models;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.ViewModels;

namespace ProjetEMIT.Controllers;

[Authorize(Roles = "Administrateur,ResponsablePedagogique")]
public class MatiereController : Controller
{
    private readonly IMatiereService _matiereService;
    private readonly IFiliereService _filiereService;
    private readonly IEnseignantService _enseignantService;

    public MatiereController(
        IMatiereService matiereService,
        IFiliereService filiereService,
        IEnseignantService enseignantService)
    {
        _matiereService = matiereService;
        _filiereService = filiereService;
        _enseignantService = enseignantService;
    }

    public async Task<IActionResult> Index(string search = "")
    {
        var matieres = await _matiereService.GetAllWithDetailsAsync(search);
        return View(matieres);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Filieres = await _filiereService.GetAllAsync();
        ViewBag.Enseignants = await _enseignantService.GetAllAsync();
        return View(new MatiereViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MatiereViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Filieres = await _filiereService.GetAllAsync();
            ViewBag.Enseignants = await _enseignantService.GetAllAsync();
            return View(model);
        }

        var matiere = new Matiere
        {
            Code = model.Code,
            Nom = model.Nom,
            Coefficient = model.Coefficient,
            NbreHeures = model.NbreHeures,
            FiliereId = model.FiliereId
        };

        var success = await _matiereService.CreateAsync(matiere, model.SelectedEnseignantsIds);

        if (success)
        {
            TempData["Success"] = "Mati&egrave;re cr&eacute;&eacute;e avec succ&egrave;s !";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Filieres = await _filiereService.GetAllAsync();
        ViewBag.Enseignants = await _enseignantService.GetAllAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var matiere = await _matiereService.GetByIdAsync(id);
        if (matiere == null) return NotFound();

        var model = new MatiereViewModel
        {
            Id = matiere.Id,
            Code = matiere.Code,
            Nom = matiere.Nom,
            Coefficient = matiere.Coefficient,
            NbreHeures = matiere.NbreHeures,
            FiliereId = matiere.FiliereId,
            SelectedEnseignantsIds = matiere.Enseignants.Select(e => e.Id).ToList()
        };

        ViewBag.Filieres = await _filiereService.GetAllAsync();
        ViewBag.Enseignants = await _enseignantService.GetAllAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MatiereViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Filieres = await _filiereService.GetAllAsync();
            ViewBag.Enseignants = await _enseignantService.GetAllAsync();
            return View(model);
        }

        var matiere = await _matiereService.GetByIdAsync(model.Id);
        if (matiere == null) return NotFound();

        matiere.Code = model.Code;
        matiere.Nom = model.Nom;
        matiere.Coefficient = model.Coefficient;
        matiere.NbreHeures = model.NbreHeures;
        matiere.FiliereId = model.FiliereId;

        var success = await _matiereService.UpdateAsync(matiere, model.SelectedEnseignantsIds);

        if (success)
        {
            TempData["Success"] = "Mati&egrave;re modifi&eacute;e avec succ&egrave;s !";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Filieres = await _filiereService.GetAllAsync();
        ViewBag.Enseignants = await _enseignantService.GetAllAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _matiereService.DeleteAsync(id);
        TempData[ok ? "Success" : "Error"] = ok
            ? "Mati&egrave;re supprim&eacute;e."
            : "Impossible de supprimer (s&eacute;ances ou donn&eacute;es li&eacute;es).";
        return RedirectToAction(nameof(Index));
    }
}
