using StudyBuddy.Domain.Models;

namespace StudyBuddy.Application.Interfaces.Auth;

public interface IJwtProvider
{
    string Generate(User user);
}