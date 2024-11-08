using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.API.Contracts.Users;

public record RegisterUserRequest(
    
    [Required] [Phone] string UserPhone,
    [Required] string UserNickName,
    [Required] string UserFullName,
    [Required] string Password
    
    );