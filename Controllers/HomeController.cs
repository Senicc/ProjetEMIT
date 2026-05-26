using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjetEMIT.Models;
using Microsoft.AspNetCore.Authorization;
using ProjetEMIT.ViewModels;

namespace ProjetEMIT.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Redirection intelligente en fonction du r&ocirc;le
            if (User.IsInRole("Administrateur") || User.IsInRole("ResponsablePedagogique"))
            {
                return RedirectToAction("Index", "Admin");
            }
            
            // Pour les enseignants et autres utilisateurs
            return RedirectToAction("Index", "EmploiDuTemps");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
