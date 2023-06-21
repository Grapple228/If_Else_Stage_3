using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record ServiceCreateRequest(
    [Required]string Name,
    [Required]string Description
    );