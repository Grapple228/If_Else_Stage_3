namespace Database.Entities;

public class Appointment : IEntity
{
    public long Id { get; init; }
    public long AccountId { get; init; }
    public long VeterinaryId { get; set; }
    public DateTime Date { get; set; }
}