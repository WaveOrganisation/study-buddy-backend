namespace StudyBuddy.API.Contracts;

public record ErrorResponse(
    int Status,
    string Message);