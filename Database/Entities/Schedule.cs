namespace Database.Entities;

public class Schedule : IEntity
{
    public long Id { get; init; }
    public long VeterinaryId { get; set; }
    public DateTime Date { get; set; }
}