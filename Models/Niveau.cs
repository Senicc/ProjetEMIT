using System.ComponentModel.DataAnnotations;        // Pour [Required], [Display], etc.
using Microsoft.AspNetCore.Identity;               // Pour Identity
using Microsoft.EntityFrameworkCore;               // Pour DbContext, DbSet
using ProjetEMIT.Models;
using ProjetEMIT.ViewModels;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Models
{
    public class Niveau
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty; // L1, L2, L3, M1, M2

        public ICollection<Classe> Classes { get; set; } = new List<Classe>();
    }
}
