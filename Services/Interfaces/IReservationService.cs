using System.Security.Claims;
using ProjetEMIT.Models;

namespace ProjetEMIT.Services.Interfaces;

public interface IReservationService
{
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task<IEnumerable<Reservation>> GetReservationsByUserAsync(string userName);
    Task<string?> GetCurrentUserIdAsync(ClaimsPrincipal user);
    Task<ServiceResult> CreateAsync(Reservation reservation);
    Task<ServiceResult> ValiderReservationAsync(int id);
    Task<ServiceResult> RefuserReservationAsync(int id, string motifRefus);
    Task<ServiceResult> AnnulerReservationAsync(int id, ClaimsPrincipal user);
}
