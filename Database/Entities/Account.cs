using Database.Enums;

namespace Database.Entities;

public class Account : IEntity
{
    public long Id { get; init; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public Roles Role { get; set; }
}