using System.ComponentModel.DataAnnotations;
using Database.Enums;

namespace Api.Requests;

public record AnimalCreateRequest(
    [Required]string Name,
    [Required]double Weight,
    [Required]double Height,
    [Required]double Lenght,
    [Required]Gender Gender
    );