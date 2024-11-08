using StudyBuddy.Domain.Models;

namespace StudyBuddy.Application.Interfaces.Repositories;

public interface IUsersRepository
{
    Task Add(User user);
    Task<User> GetByPhone(string phone);
}