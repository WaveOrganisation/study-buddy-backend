namespace StudyBuddy.API.Contracts.Users;

public record SendConfirmationCodeRequest
{
    public string PhoneNumber { get; init; }
}