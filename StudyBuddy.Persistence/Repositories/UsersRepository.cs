using AutoMapper;
using StudyBuddy.Application.Interfaces.Repositories;
using StudyBuddy.Domain.Models;
using StudyBuddy.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace StudyBuddy.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly StudyBuddyDbContext _context;
    private readonly IMapper _mapper;

    public UsersRepository(StudyBuddyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Add(User user)
    {
        var userEntity = new UserEntity()
        {
            Id = user.Id,
            UserName = user.UserName,
            PasswordHash = user.PasswordHash,
            Phone = user.Phone
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetByPhone(string phone)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Phone == phone) ?? throw new Exception();

        return _mapper.Map<User>(userEntity);
    }
}