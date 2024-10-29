using StudyBuddy.Application.Interfaces.Auth;
using StudyBuddy.Domain.Models;

namespace StudyBuddy.Application.Services;

public class UsersService
{
    private readonly IPasswordHasher _passwordHasher;
    
    public UsersService(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }
    
    public async Task Register(string userPhone, string userName, string userFullName, string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);
        
        
        
    }
}