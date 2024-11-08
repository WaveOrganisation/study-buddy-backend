using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.API.Contracts.Users;

public record VerifyConfirmationCodeRequest
{
    [Required] [Phone] public string PhoneNumber { get; init; }
    [Required] public string ConfirmationCode { get; init; }
    [Required] public string UserNickName { get; init; }   
    [Required] public string UserFullName { get; init; }   
    [Required] public string Password { get; init; } 
}