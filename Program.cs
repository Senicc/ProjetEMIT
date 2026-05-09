using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProjetEMIT.Data;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;
using ProjetEMIT.Repositories.Implementations;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.Helpers;
using ProjetEMIT.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// =============================================
// Configuration de la base de données PostgreSQL
// =============================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));

    // Meilleures pratiques pour PostgreSQL
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


// =============================================
// Configuration ASP.NET Identity
// =============================================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false; // À activer en production
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configuration des cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(3);
    options.SlidingExpiration = true;
});


// =============================================
// Services & Repositories (Dependency Injection)
// =============================================

// Repositories
builder.Services.AddScoped<ISalleRepository, SalleRepository>();
builder.Services.AddScoped<IEnseignantRepository, EnseignantRepository>();
builder.Services.AddScoped<IMatiereRepository, MatiereRepository>();
builder.Services.AddScoped<IFiliereRepository, FiliereRepository>();
builder.Services.AddScoped<IClasseRepository, ClasseRepository>();
builder.Services.AddScoped<ISeanceRepository, SeanceRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ICreneauRepository, CreneauRepository>();

// Services (Business Logic)
builder.Services.AddScoped<ISalleService, SalleService>();
builder.Services.AddScoped<IEmploiDuTempsService, EmploiDuTempsService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IEnseignantService, EnseignantService>();
builder.Services.AddScoped<IRapportService, RapportService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// =============================================
// MVC + Autres services
// =============================================
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(); // Utile pendant le développement

// Ajout de la session (si besoin pour notifications temporaires)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// =============================================
// Pipeline HTTP
// =============================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   // Important : Avant Authorization
app.UseAuthorization();

app.UseSession();

// =============================================
// Routes
// =============================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// =============================================
// Seed des rôles et données initiales
// =============================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync(); // Applique les migrations
        await SeedData.InitializeAsync(services); // À créer dans Helpers/SeedData.cs
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Une erreur est survenue lors du Seed de la base de données.");
    }
}

app.Run();