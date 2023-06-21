namespace Api.Requests;

public record VisitUpdateRequest(
    DateTime Date,
    long VeterinaryId
    );