using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;
using ProjetEMIT.Services.Interfaces;

namespace ProjetEMIT.Services.Implementations;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ISeanceRepository _seanceRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReservationService(
        IReservationRepository reservationRepository,
        ISeanceRepository seanceRepository,
        UserManager<ApplicationUser> userManager)
    {
        _reservationRepository = reservationRepository;
        _seanceRepository = seanceRepository;
        _userManager = userManager;
    }

    public Task<IEnumerable<Reservation>> GetAllAsync() =>
        _reservationRepository.GetAllWithDetailsAsync();

    public async Task<IEnumerable<Reservation>> GetReservationsByUserAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
            return Array.Empty<Reservation>();

        return await _reservationRepository.GetByDemandeurIdAsync(user.Id);
    }

    public Task<string?> GetCurrentUserIdAsync(ClaimsPrincipal user) =>
        Task.FromResult(user.FindFirstValue(ClaimTypes.NameIdentifier));

    public async Task<ServiceResult> CreateAsync(Reservation reservation)
    {
        if (reservation.HeureDebut >= reservation.HeureFin)
            return new ServiceResult(false, "L'heure de fin doit être après l'heure de début.");

        if (string.IsNullOrEmpty(reservation.DemandeurId))
            return new ServiceResult(false, "Utilisateur non identifié.");

        if (await _reservationRepository.ChevaucheAsync(reservation.SalleId, reservation.Date, reservation.HeureDebut, reservation.HeureFin))
            return new ServiceResult(false, "La salle est déjà réservée sur ce créneau.");

        if (await _seanceRepository.SalleOccupeeParSeanceAsync(reservation.SalleId, reservation.Date, reservation.HeureDebut, reservation.HeureFin))
            return new ServiceResult(false, "Un cours est déjà programmé sur ce créneau pour cette salle.");

        await _reservationRepository.AddAsync(reservation);
        return new ServiceResult(true, "OK");
    }

    public async Task<ServiceResult> ValiderReservationAsync(int id)
    {
        var r = await _reservationRepository.GetByIdAsync(id);
        if (r == null) return new ServiceResult(false, "Réservation introuvable.");

        if (await _seanceRepository.SalleOccupeeParSeanceAsync(r.SalleId, r.Date, r.HeureDebut, r.HeureFin))
            return new ServiceResult(false, "Impossible de valider : un cours occupe déjà cette salle.");

        r.Statut = "Validee";
        await _reservationRepository.UpdateAsync(r);
        return new ServiceResult(true, "Réservation validée.");
    }

    public async Task<ServiceResult> RefuserReservationAsync(int id, string motifRefus)
    {
        var r = await _reservationRepository.GetByIdAsync(id);
        if (r == null) return new ServiceResult(false, "Réservation introuvable.");

        r.Statut = "Refusee";
        if (!string.IsNullOrWhiteSpace(motifRefus))
            r.Motif = string.IsNullOrEmpty(r.Motif) ? $"Refus : {motifRefus}" : $"{r.Motif} | Refus : {motifRefus}";

        await _reservationRepository.UpdateAsync(r);
        return new ServiceResult(true, "Réservation refusée.");
    }

    public async Task<ServiceResult> AnnulerReservationAsync(int id, ClaimsPrincipal user)
    {
        var r = await _reservationRepository.GetByIdAsync(id);
        if (r == null) return new ServiceResult(false, "Réservation introuvable.");

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = user.IsInRole("Administrateur") || user.IsInRole("ResponsablePedagogique");
        if (!isAdmin && r.DemandeurId != userId)
            return new ServiceResult(false, "Vous ne pouvez pas annuler cette réservation.");

        r.Statut = "Annulee";
        await _reservationRepository.UpdateAsync(r);
        return new ServiceResult(true, "Réservation annulée.");
    }
}
