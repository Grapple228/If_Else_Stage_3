namespace Api.Dtos;

public record VisitDto(
        long Id,
        long VisiterId,
        long VeterinaryId,
        DateTime Date
);