using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Data;
using ProjetEMIT.Models;
using ProjetEMIT.Models.Enums;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Repositories.Implementations;

public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Reservation>> GetAllWithDetailsAsync()
    {
        return await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Salle)
            .Include(r => r.Demandeur)
            .OrderByDescending(r => r.DateDemande)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByDemandeurIdAsync(string demandeurId)
    {
        return await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Salle)
            .Include(r => r.Demandeur)
            .Where(r => r.DemandeurId == demandeurId)
            .OrderByDescending(r => r.DateDemande)
            .ToListAsync();
    }

    public async Task<bool> ChevaucheAsync(int salleId, DateOnly date, TimeOnly debut, TimeOnly fin, int? excludeReservationId = null)
    {
        return await _context.Reservations.AnyAsync(r =>
            r.SalleId == salleId &&
            r.Date == date &&
            r.Statut != ReservationStatus.Refusee &&
            r.Statut != ReservationStatus.Annulee &&
            r.HeureDebut < fin &&
            r.HeureFin > debut &&
            (excludeReservationId == null || r.Id != excludeReservationId));
    }

    public Task<bool> ExistsActiveForSalleAsync(int salleId) =>
        _context.Reservations.AnyAsync(r =>
            r.SalleId == salleId &&
            r.Statut != ReservationStatus.Refusee &&
            r.Statut != ReservationStatus.Annulee);
}
