using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record RegistrationRequest(
    [Required]string Username,
    [Required]string Password,
    [Required]string FirstName,
    [Required]string LastName
    );