namespace StudyBuddy.Domain.Models;

public class User
{
    // User will have specific role like... Student \ Teacher \ Teh Admin etc.
    
    public Guid Id {get; set;}
    public string Name {get; set;}
    public string HashPassword {get; set;}
    
    public string UserRole { get; set;}
    
}

