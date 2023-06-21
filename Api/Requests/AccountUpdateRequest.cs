using System.ComponentModel.DataAnnotations;
using Database.Enums;

namespace Api.Requests;

public record AccountUpdateRequest(
    [Required]string Username,
    [Required]string Password,
    [Required]string FirstName,
    [Required]string LastName,
    [Required]Roles Role
    );