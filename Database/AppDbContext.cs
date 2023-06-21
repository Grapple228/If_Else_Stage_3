using Database.Entities;
using Database.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Database;

public sealed class AppDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Animal> Animals { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<Schedule> Schedules { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<Visit> Visits { get; set; } = null!;
    public DbSet<Veterinary> Veterinaries { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIdentityColumns();
        modelBuilder.Entity<Account>()
            .Property(d => d.Role)
            .HasConversion(new EnumToStringConverter<Roles>());
        modelBuilder.Entity<Animal>()
            .Property(d => d.Gender)
            .HasConversion(new EnumToStringConverter<Gender>());
        new DbInitializer(modelBuilder).Seed();
        base.OnModelCreating(modelBuilder);
    }
}