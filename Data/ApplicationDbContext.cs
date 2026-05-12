using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Models;

namespace ProjetEMIT.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Salle> Salles => Set<Salle>();
    public DbSet<Enseignant> Enseignants => Set<Enseignant>();
    public DbSet<Matiere> Matieres => Set<Matiere>();
    public DbSet<Filiere> Filieres => Set<Filiere>();
    public DbSet<Niveau> Niveaux => Set<Niveau>();
    public DbSet<Classe> Classes => Set<Classe>();
    public DbSet<Creneau> Creneaux => Set<Creneau>();
    public DbSet<Seance> Seances => Set<Seance>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Enseignant>()
            .HasMany(e => e.Matieres)
            .WithMany(m => m.Enseignants);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Demandeur)
            .WithMany()
            .HasForeignKey(r => r.DemandeurId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Seance>()
            .HasIndex(s => new { s.Date, s.CreneauId, s.SalleId });

        modelBuilder.Entity<Seance>()
            .HasIndex(s => new { s.Date, s.CreneauId, s.EnseignantId });
    }
}
