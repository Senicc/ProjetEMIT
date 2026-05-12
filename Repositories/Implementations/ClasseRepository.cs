using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Data;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Repositories.Implementations;

public class ClasseRepository : BaseRepository<Classe>, IClasseRepository
{
    public ClasseRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Classe>> GetAllWithDetailsAsync()
    {
        return await _context.Classes
            .AsNoTracking()
            .Include(c => c.Filiere)
            .Include(c => c.Niveau)
            .OrderBy(c => c.Filiere!.Nom).ThenBy(c => c.Niveau!.Nom).ThenBy(c => c.Nom)
            .ToListAsync();
    }
}
