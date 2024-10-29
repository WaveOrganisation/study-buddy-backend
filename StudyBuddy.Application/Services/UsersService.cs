using StudyBuddy.Application.Interfaces.Auth;
using StudyBuddy.Domain.Models;

namespace StudyBuddy.Application.Services;

public class UsersService
{
    private readonly IPasswordHasher _passwordHasher;
    
    public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }
    
    public async Task Register(string userNickName, string userFullName, string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);
        
        var user = User.Create(Guid.NewGuid(), userNickName, userFullName, hashedPassword);
        
        await _usersRepository.Add(user);
    }
}