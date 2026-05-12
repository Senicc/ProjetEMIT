using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetEMIT.Repositories.Interfaces;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.ViewModels;

namespace ProjetEMIT.Controllers;

[Authorize(Roles = "Administrateur")]
public class AdminController : Controller
{
    private readonly ISalleService _salleService;
    private readonly IEnseignantService _enseignantService;
    private readonly IEmploiDuTempsService _emploiService;

    public AdminController(
        ISalleService salleService,
        IEnseignantService enseignantService,
        IEmploiDuTempsService emploiService)
    {
        _salleService = salleService;
        _enseignantService = enseignantService;
        _emploiService = emploiService;
    }

    public async Task<IActionResult> Index()
    {
        var topSalles = await _salleService.GetSallesLesPlusUtiliseesAsync(5);
        var dashboard = new AdminDashboardViewModel
        {
            TotalSalles = (await _salleService.GetAllSallesAsync()).Count(),
            TotalEnseignants = await _enseignantService.GetTotalCountAsync(),
            TotalClasses = await _enseignantService.GetTotalClassesAsync(),
            TotalSeancesAujourdHui = await _emploiService.GetSeancesDuJourCountAsync(),
            TauxOccupation = await _salleService.GetTauxOccupationAsync(),
            SallesLesPlusUtilisees = topSalles.Select(s => new SalleStat
            {
                NomSalle = s.NomSalle,
                NombreSeances = s.NombreSeances,
                TauxOccupation = 0
            }).ToList()
        };

        return View(dashboard);
    }

    [HttpGet]
    public async Task<JsonResult> GetOccupationStats()
    {
        var (labels, values) = await _salleService.GetOccupationChartSeriesAsync(7);
        return Json(new { labels, values });
    }
}
