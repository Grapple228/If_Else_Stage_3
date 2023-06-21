using Database.Enums;

namespace Database.Entities;

public class Animal : IEntity
{
    public long Id { get; init; }
    public string Name { get; set; }
    public double Weight { get; set; }
    public double Height { get; set; }
    public double Length { get; set; }
    public long OwnerId { get; set; }
    public Gender Gender { get; set; }
}