using Database.Enums;

namespace Api.Dtos;

public record AccountDto(
    long Id,
    string Username,
    string FirstName,
    string LastName,
    Roles Role
);