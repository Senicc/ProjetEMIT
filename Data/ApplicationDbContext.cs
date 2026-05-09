public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Salle> Salles { get; set; }
    public DbSet<Enseignant> Enseignants { get; set; }
    public DbSet<Matiere> Matieres { get; set; }
    public DbSet<Filiere> Filieres { get; set; }
    public DbSet<Niveau> Niveaux { get; set; }
    public DbSet<Classe> Classes { get; set; }
    public DbSet<Creneau> Creneaux { get; set; }
    public DbSet<Seance> Seances { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurations des relations Many-to-Many
        modelBuilder.Entity<Enseignant>()
            .HasMany(e => e.Matieres)
            .WithMany(m => m.Enseignants);

        // Index pour optimisation
        modelBuilder.Entity<Seance>()
            .HasIndex(s => new { s.Date, s.CreneauId, s.SalleId });

        modelBuilder.Entity<Seance>()
            .HasIndex(s => new { s.Date, s.CreneauId, s.EnseignantId });
    }
}