using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record ScheduleUpdateRequest([Required]DateTime Date);