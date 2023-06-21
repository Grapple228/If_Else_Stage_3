using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record AppointmentUpdateRequest([Required]DateTime Date, long VeterinaryId);