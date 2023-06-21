namespace Api.Dtos;

public record AppointmentDto(
        long Id,
        long AccountId,
        DateTime Date,
        long VeterinaryId
);