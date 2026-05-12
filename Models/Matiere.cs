using System.ComponentModel.DataAnnotations;        // Pour [Required], [Display], etc.
using Microsoft.AspNetCore.Identity;               // Pour Identity
using Microsoft.EntityFrameworkCore;               // Pour DbContext, DbSet
using ProjetEMIT.Models;
using ProjetEMIT.ViewModels;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Models
{
    public class Matiere
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // ex: INF101
        public string Nom { get; set; } = string.Empty;
        public int Coefficient { get; set; }
        public int NbreHeures { get; set; }

        public int FiliereId { get; set; }
        public Filiere Filiere { get; set; } = null!;

        public ICollection<Enseignant> Enseignants { get; set; } = new List<Enseignant>();
        public ICollection<Seance> Seances { get; set; } = new List<Seance>();
    }
}
