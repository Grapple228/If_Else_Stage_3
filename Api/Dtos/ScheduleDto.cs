namespace Api.Dtos;

public record ScheduleDto(
        long Id,
        long VeterinaryId,
        DateTime Date
    );