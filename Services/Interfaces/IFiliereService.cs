using ProjetEMIT.Models;

namespace ProjetEMIT.Services.Interfaces;

public interface IFiliereService
{
    Task<IEnumerable<Filiere>> GetAllAsync();
}
