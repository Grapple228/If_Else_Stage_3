using Database.Entities;
using Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class DbInitializer
{
    private readonly ModelBuilder _modelBuilder;

    public DbInitializer(ModelBuilder modelBuilder)
    {
        _modelBuilder = modelBuilder;
    }

    public void Seed()
    {
        _modelBuilder.Entity<Account>()
            .HasData(
                new Account()
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin",
                    Role = Roles.ADMIN,
                    FirstName = "Admin",
                    LastName = "Admin"
                }
        );
    }
}