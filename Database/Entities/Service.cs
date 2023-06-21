namespace Database.Entities;

public class Service : IEntity
{
    public long Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long VeterinaryId { get; set; }
}