using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.ViewModels;

namespace ProjetEMIT.Controllers;

[Authorize]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly ISalleService _salleService;

    public ReservationController(IReservationService reservationService, ISalleService salleService)
    {
        _reservationService = reservationService;
        _salleService = salleService;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<Reservation> reservations;

        if (User.IsInRole("Administrateur") || User.IsInRole("ResponsablePedagogique"))
            reservations = await _reservationService.GetAllAsync();
        else
            reservations = await _reservationService.GetReservationsByUserAsync(User.Identity?.Name ?? "");

        return View(reservations);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Salles = await _salleService.GetAllSallesAsync();
        return View(new ReservationViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Salles = await _salleService.GetAllSallesAsync();
            return View(model);
        }

        var userId = await _reservationService.GetCurrentUserIdAsync(User);
        if (string.IsNullOrEmpty(userId))
        {
            ModelState.AddModelError("", "Utilisateur non authentifiù.");
            ViewBag.Salles = await _salleService.GetAllSallesAsync();
            return View(model);
        }

        var reservation = new Reservation
        {
            SalleId = model.SalleId,
            Date = model.Date,
            HeureDebut = model.HeureDebut,
            HeureFin = model.HeureFin,
            Motif = model.Motif,
            DemandeurId = userId,
            Statut = "EnAttente"
        };

        var result = await _reservationService.CreateAsync(reservation);

        if (result.Success)
        {
            TempData["Success"] = "Rùservation soumise avec succùs ! En attente de validation.";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", result.Message);
        ViewBag.Salles = await _salleService.GetAllSallesAsync();
        return View(model);
    }

    [Authorize(Roles = "Administrateur,ResponsablePedagogique")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Valider(int id)
    {
        var result = await _reservationService.ValiderReservationAsync(id);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrateur,ResponsablePedagogique")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Refuser(int id, string motifRefus)
    {
        var result = await _reservationService.RefuserReservationAsync(id, motifRefus);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Annuler(int id)
    {
        var result = await _reservationService.AnnulerReservationAsync(id, User);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
}
