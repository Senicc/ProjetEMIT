using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Controllers;

[Authorize(Roles = "Administrateur,ResponsablePedagogique")]
public class ClasseController : Controller
{
    private readonly IClasseRepository _classeRepository;

    public ClasseController(IClasseRepository classeRepository)
    {
        _classeRepository = classeRepository;
    }

    public async Task<IActionResult> Index()
    {
        var classes = await _classeRepository.GetAllWithDetailsAsync();
        return View(classes);
    }
}
