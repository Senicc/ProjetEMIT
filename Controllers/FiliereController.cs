using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Controllers;

[Authorize(Roles = "Administrateur,ResponsablePedagogique")]
public class FiliereController : Controller
{
    private readonly IFiliereRepository _filiereRepository;

    public FiliereController(IFiliereRepository filiereRepository)
    {
        _filiereRepository = filiereRepository;
    }

    public async Task<IActionResult> Index()
    {
        var filieres = await _filiereRepository.GetAllAsync();
        return View(filieres);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Filiere());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Filiere filiere)
    {
        if (ModelState.IsValid)
        {
            await _filiereRepository.AddAsync(filiere);
            TempData["Success"] = "Fili&egrave;re cr&eacute;&eacute;e avec succ&egrave;s !";
            return RedirectToAction(nameof(Index));
        }
        return View(filiere);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var filiere = await _filiereRepository.GetByIdAsync(id);
        if (filiere == null) return NotFound();
        return View(filiere);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Filiere filiere)
    {
        if (ModelState.IsValid)
        {
            await _filiereRepository.UpdateAsync(filiere);
            TempData["Success"] = "Fili&egrave;re modifi&eacute;e avec succ&egrave;s !";
            return RedirectToAction(nameof(Index));
        }
        return View(filiere);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _filiereRepository.DeleteAsync(id);
        TempData["Success"] = "Fili&egrave;re supprim&eacute;e avec succ&egrave;s !";
        return RedirectToAction(nameof(Index));
    }
}
