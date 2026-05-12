using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Controllers;

[Authorize(Roles = "Administrateur,ResponsablePedagogique")]
public class SalleController : Controller
{
    private readonly ISalleService _salleService;
    private readonly IEmploiDuTempsService _emploiService;

    public SalleController(ISalleService salleService, IEmploiDuTempsService emploiService)
    {
        _salleService = salleService;
        _emploiService = emploiService;
    }

    public async Task<IActionResult> Index(string search = "")
    {
        ViewBag.SearchTerm = search;

        var salles = string.IsNullOrEmpty(search)
            ? await _salleService.GetAllSallesAsync()
            : await _salleService.SearchAsync(search);

        return View(salles);
    }

    public async Task<IActionResult> Details(int id)
    {
        var salle = await _salleService.GetByIdAsync(id);
        if (salle == null) return NotFound();

        var today = DateOnly.FromDateTime(DateTime.Today);
        ViewBag.SeancesAujourdhui = await _emploiService.GetEmploiParSalleAsync(id, today, today);

        return View(salle);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Salle salle)
    {
        if (!ModelState.IsValid)
            return View(salle);

        var success = await _salleService.CreateAsync(salle);
        if (success)
        {
            TempData["Success"] = "Salle ajoutée avec succès !";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", "Erreur lors de l'ajout de la salle.");
        return View(salle);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var salle = await _salleService.GetByIdAsync(id);
        if (salle == null) return NotFound();
        return View(salle);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Salle salle)
    {
        if (id != salle.Id) return NotFound();

        if (!ModelState.IsValid)
            return View(salle);

        var success = await _salleService.UpdateAsync(salle);
        if (success)
        {
            TempData["Success"] = "Salle modifiée avec succès !";
            return RedirectToAction(nameof(Index));
        }

        return View(salle);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _salleService.DeleteAsync(id);
        if (success)
            TempData["Success"] = "Salle supprimée avec succès !";
        else
            TempData["Error"] = "Impossible de supprimer cette salle (elle est peut-être utilisée).";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Disponibilite(DateOnly? date)
    {
        var selectedDate = date ?? DateOnly.FromDateTime(DateTime.Today);
        ViewBag.SelectedDate = selectedDate;

        var salles = await _salleService.GetSallesDisponiblesAsync(selectedDate,
            new TimeOnly(8, 0), new TimeOnly(18, 0));

        return View(salles);
    }
}
