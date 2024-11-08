using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.API.Contracts.Users;


public record LoginUserRequest(
    [Required] string Phone,
    [Required] string Password);