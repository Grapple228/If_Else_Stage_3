namespace Api.Requests;

public record VisitCreateRequest(
        DateTime Date,
        long VeterinaryId
    );