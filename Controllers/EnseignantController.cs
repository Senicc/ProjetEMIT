using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetEMIT.Models;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.ViewModels;

namespace ProjetEMIT.Controllers;

[Authorize(Roles = "Administrateur,ResponsablePedagogique")]
public class EnseignantController : Controller
{
    private readonly IEnseignantService _enseignantService;
    private readonly IMatiereService _matiereService;

    public EnseignantController(IEnseignantService enseignantService, IMatiereService matiereService)
    {
        _enseignantService = enseignantService;
        _matiereService = matiereService;
    }

    public async Task<IActionResult> Index(string search = "")
    {
        ViewBag.SearchTerm = search;
        var enseignants = await _enseignantService.GetAllAsync(search);
        return View(enseignants);
    }

    public async Task<IActionResult> Details(int id)
    {
        var enseignant = await _enseignantService.GetWithMatieresAsync(id);
        if (enseignant == null) return NotFound();

        ViewBag.Seances = await _enseignantService.GetSeancesAsync(id);
        return View(enseignant);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Matieres = await _matiereService.GetAllAsync();
        return View(new EnseignantViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EnseignantViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Matieres = await _matiereService.GetAllAsync();
            return View(model);
        }

        var enseignant = new Enseignant
        {
            Nom = model.Nom,
            Prenom = model.Prenom,
            Email = model.Email,
            Telephone = model.Telephone,
            Specialite = model.Specialite
        };

        var success = await _enseignantService.CreateAsync(enseignant, model.SelectedMatieresIds);

        if (success)
        {
            TempData["Success"] = "Enseignant ajouté avec succès !";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", "Erreur lors de la création.");
        ViewBag.Matieres = await _matiereService.GetAllAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var enseignant = await _enseignantService.GetWithMatieresAsync(id);
        if (enseignant == null) return NotFound();

        var model = new EnseignantViewModel
        {
            Id = enseignant.Id,
            Nom = enseignant.Nom,
            Prenom = enseignant.Prenom,
            Email = enseignant.Email,
            Telephone = enseignant.Telephone,
            Specialite = enseignant.Specialite,
            SelectedMatieresIds = enseignant.Matieres.Select(m => m.Id).ToList()
        };

        ViewBag.Matieres = await _matiereService.GetAllAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EnseignantViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Matieres = await _matiereService.GetAllAsync();
            return View(model);
        }

        var success = await _enseignantService.UpdateAsync(model);
        if (success)
        {
            TempData["Success"] = "Enseignant modifié avec succès !";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Matieres = await _matiereService.GetAllAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _enseignantService.DeleteAsync(id);
        if (success)
            TempData["Success"] = "Enseignant supprimé avec succès !";
        else
            TempData["Error"] = "Impossible de supprimer cet enseignant (il a des séances programmées).";

        return RedirectToAction(nameof(Index));
    }
}
