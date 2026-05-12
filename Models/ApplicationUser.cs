using System.ComponentModel.DataAnnotations;        // Pour [Required], [Display], etc.
using Microsoft.AspNetCore.Identity;               // Pour Identity
using Microsoft.EntityFrameworkCore;               // Pour DbContext, DbSet
using ProjetEMIT.Models;
using ProjetEMIT.ViewModels;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.Repositories.Interfaces;


namespace ProjetEMIT.Models
{
    // Models/ApplicationUser.cs
    public class ApplicationUser : IdentityUser
    {
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public int? EnseignantId { get; set; }        // Lien avec Enseignant si rôle Enseignant
        public Enseignant? Enseignant { get; set; }
    }
}
