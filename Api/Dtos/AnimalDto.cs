using Database.Enums;

namespace Api.Dtos;

public record AnimalDto(
    long Id,
    string Name,
    double Weight,
    double Height,
    double Lenght,
    long OwnerId,
    Gender Gender
    );