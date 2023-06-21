using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record ScheduleCreateRequest([Required]DateTime Date);
