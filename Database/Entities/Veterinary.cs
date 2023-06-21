namespace Database.Entities;

public class Veterinary : IEntity
{
    public long Id { get; init; }
    public long AccountId { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public virtual ICollection<Service> Services { get; } = new List<Service>();
}