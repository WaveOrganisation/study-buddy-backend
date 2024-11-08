using System.Collections.Concurrent;
using StudyBuddy.Application.Interfaces.Auth;
using StudyBuddy.Application.Interfaces.Repositories;
using StudyBuddy.Domain.Models;

namespace StudyBuddy.Application.Services;
public class UserService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    
    private static readonly ConcurrentDictionary<string, string> _confirmationCodes = new();    
    public UserService(
        IUsersRepository usersRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }
    
    public string GenerateConfirmationCode(string phoneNumber)
    {
        var random = new Random();
        var code = random.Next(1000, 9999).ToString();
        _confirmationCodes[phoneNumber] = code;
        return code;
    }
    
    public bool VerifyConfirmationCode(string phoneNumber, string code)
    {
        return _confirmationCodes.ContainsKey(phoneNumber) && _confirmationCodes[phoneNumber] == code;
    }
    
    public async Task Register(string userNickName, string password, string userFullName, string phoneNumber)
    {
        var hashedPassword = _passwordHasher.Generate(password);

        var user = User.Create(
            Guid.NewGuid(),
            userNickName,
            hashedPassword,
            userFullName,
            phoneNumber
            );

        await _usersRepository.Add(user);
    }

    public async Task<string> Login(string phone, string password)
    {
        var user = await _usersRepository.GetByPhone(phone);

        var result = _passwordHasher.Verify(password, user.PasswordHash);

        if (result == false)
        {
            throw new Exception("Failed to login");
        }

        var token = _jwtProvider.Generate(user);

        return token;
    }
    
    public Task StoreConfirmationCode(string phoneNumber, string confirmationCode)
    {
        _confirmationCodes[phoneNumber] = confirmationCode;
        return Task.CompletedTask;
    }

    public Task<string?> GetStoredConfirmationCode(string phoneNumber)
    {
        _confirmationCodes.TryGetValue(phoneNumber, out var code);
        return Task.FromResult(code);
    }

    public async Task<User?> GetByPhoneNumber(string phoneNumber)
    {
        return await _usersRepository.GetByPhone(phoneNumber);
    }
}