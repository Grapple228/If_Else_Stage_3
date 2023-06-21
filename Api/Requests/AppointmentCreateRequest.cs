using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record AppointmentCreateRequest([Required]DateTime Date, long VeterinaryId);
