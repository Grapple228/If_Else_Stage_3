namespace Database.Entities;

public class Visit : IEntity
{
    public long Id { get; init; }
    public long VisiterId { get; set; }
    public long VeterinaryId { get; set; }
    public DateTime Date { get; set; }
}