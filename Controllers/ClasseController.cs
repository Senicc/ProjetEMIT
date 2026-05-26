using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Data;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;
using ProjetEMIT.ViewModels;

namespace ProjetEMIT.Controllers;

[Authorize(Roles = "Administrateur,ResponsablePedagogique")]
public class ClasseController : Controller
{
    private readonly IClasseRepository _classeRepository;
    private readonly IFiliereRepository _filiereRepository;
    private readonly ApplicationDbContext _context;

    public ClasseController(IClasseRepository classeRepository, IFiliereRepository filiereRepository, ApplicationDbContext context)
    {
        _classeRepository = classeRepository;
        _filiereRepository = filiereRepository;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var classes = await _classeRepository.GetAllWithDetailsAsync();
        return View(classes);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateLookupsAsync();
        return View(new ClasseViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClasseViewModel model)
    {
        if (ModelState.IsValid)
        {
            var classe = new Classe { Nom = model.Nom, FiliereId = model.FiliereId, NiveauId = model.NiveauId };
            await _classeRepository.AddAsync(classe);
            TempData["Success"] = "Classe cr&eacute;&eacute;e avec succ&egrave;s !";
            return RedirectToAction(nameof(Index));
        }
        await PopulateLookupsAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var classe = await _classeRepository.GetByIdAsync(id);
        if (classe == null) return NotFound();

        var model = new ClasseViewModel { Id = classe.Id, Nom = classe.Nom, FiliereId = classe.FiliereId, NiveauId = classe.NiveauId };
        await PopulateLookupsAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ClasseViewModel model)
    {
        if (ModelState.IsValid)
        {
            var classe = await _classeRepository.GetByIdAsync(model.Id);
            if (classe == null) return NotFound();
            classe.Nom = model.Nom;
            classe.FiliereId = model.FiliereId;
            classe.NiveauId = model.NiveauId;
            await _classeRepository.UpdateAsync(classe);
            TempData["Success"] = "Classe modifi&eacute;e avec succ&egrave;s !";
            return RedirectToAction(nameof(Index));
        }
        await PopulateLookupsAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _classeRepository.DeleteAsync(id);
        TempData["Success"] = "Classe supprim&eacute;e avec succ&egrave;s !";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateLookupsAsync()
    {
        ViewBag.Filieres = await _filiereRepository.GetAllAsync();
        ViewBag.Niveaux = await _context.Niveaux.AsNoTracking().OrderBy(n => n.Nom).ToListAsync();
    }
}
