using StudyBuddy.Domain.Models;

namespace StudyBuddy.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(ApplicationUser user);
    }
}
