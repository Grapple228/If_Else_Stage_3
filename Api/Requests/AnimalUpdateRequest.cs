using System.ComponentModel.DataAnnotations;
using Database.Enums;

namespace Api.Requests;

public record AnimalUpdateRequest(
    [Required]string Name,
    [Required]double Weight,
    [Required]double Height,
    [Required]double Length,
    [Required]long OwnerId,
    [Required]Gender Gender
    );