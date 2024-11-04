using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Concurrent;
using StudyBuddy.Domain.Models;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();
    private static ConcurrentDictionary<string, string> _verificationCodes = new ConcurrentDictionary<string, string>();
    
    // Test phone number and verification code for demo purposes
    private const string TestPhoneNumber = "1234567890";
    private const string TestVerificationCode = "1234";

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterModel model)
    {
        if (model.PhoneNumber != TestPhoneNumber)
        {
            return BadRequest("Only the test phone number is accepted for registration.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserNumber = new Random().Next(1000, 9999),
            UserNickName = model.UserNickName,
            UserFullName = model.UserFullName,
            HashPassword = HashPassword(model.Password)
        };

        _users.TryAdd(user.UserNumber.ToString(), user);

        // Send test verification code (1234)
        _verificationCodes[model.PhoneNumber] = TestVerificationCode;
        return Ok("User registered. Verification code sent.");
    }

    // Verify phone number with code
    [HttpPost("verify")]
    public IActionResult VerifyPhone([FromBody] VerifyModel model)
    {
        if (!_verificationCodes.TryGetValue(model.PhoneNumber, out var expectedCode) || expectedCode != model.Code)
        {
            return BadRequest("Invalid verification code.");
        }

        // Code is valid, remove from dictionary
        _verificationCodes.TryRemove(model.PhoneNumber, out _);
        return Ok("Phone number verified.");
    }

    // Login with phone number and password
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        if (!_users.Values.Any(u => u.UserNumber.ToString() == model.UserNumber && VerifyPassword(model.Password, u.HashPassword)))
        {
            return Unauthorized("Invalid user number or password.");
        }

        return Ok("Login successful.");
    }

    // method to hash password
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    // method to verify password
    private bool VerifyPassword(string password, string hashPassword)
    {
        return HashPassword(password) == hashPassword;
    }
}


public class RegisterModel
{
    public string PhoneNumber { get; set; }
    public string UserNickName { get; set; }
    public string UserFullName { get; set; }
    public string Password { get; set; }
}


public class VerifyModel
{
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
}


public class LoginModel
{
    public string UserNumber { get; set; }
    public string Password { get; set; }
}
