using System.ComponentModel.DataAnnotations;        // Pour [Required], [Display], etc.
using Microsoft.AspNetCore.Identity;               // Pour Identity
using Microsoft.EntityFrameworkCore;               // Pour DbContext, DbSet
using ProjetEMIT.Models;
using ProjetEMIT.ViewModels;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Models
{
    public class Creneau
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;        // ex: Créneau 1
        public TimeOnly HeureDebut { get; set; }
        public TimeOnly HeureFin { get; set; }

        public ICollection<Seance> Seances { get; set; } = new List<Seance>();
    }
}
