using System.ComponentModel.DataAnnotations;        // Pour [Required], [Display], etc.
using Microsoft.AspNetCore.Identity;               // Pour Identity
using Microsoft.EntityFrameworkCore;               // Pour DbContext, DbSet
using ProjetEMIT.Models;
using ProjetEMIT.ViewModels;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Models
{
    public class Filiere
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // ex: INFO, GEST
        public string Nom { get; set; } = string.Empty;   // Informatique, Gestion, etc.

        public ICollection<Matiere> Matieres { get; set; } = new List<Matiere>();
        public ICollection<Classe> Classes { get; set; } = new List<Classe>();
    }
}
