using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record ServiceUpdateRequest(
    [Required]string Name,
    [Required]string Description
);