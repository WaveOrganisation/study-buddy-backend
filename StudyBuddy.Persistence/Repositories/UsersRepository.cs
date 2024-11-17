using AutoMapper;
using StudyBuddy.Application.Interfaces.Repositories;
using StudyBuddy.Domain.Models;
using StudyBuddy.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Application.Interfaces.Auth;

namespace StudyBuddy.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly StudyBuddyDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;


    public UsersRepository(StudyBuddyDbContext context, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _context = context;
        _mapper = mapper;
        _passwordHasher = passwordHasher; 
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
    
    public async Task<bool> ExistsByPhoneNumber(string phoneNumber)
    {
        return await _context.Users.AnyAsync(u => u.Phone == phoneNumber);
    }
    
    public async Task Update(User user)
    {
        // Retrieve the UserEntity from the database
        var userEntity = await _context.Users
                             .FirstOrDefaultAsync(u => u.Id == user.Id) // Ensure we update the user by Id
                         ?? throw new Exception("User not found");

        // Map the User domain model to the UserEntity
        userEntity.UserName = user.UserName;
        userEntity.PasswordHash = user.PasswordHash;
        userEntity.Phone = user.Phone;

        // Update the UserEntity in the database
        _context.Users.Update(userEntity);
        await _context.SaveChangesAsync();
    }

    
    /*
    public async Task Update(UserEntity userEntity)
    {
        _context.Users.Update(userEntity);
        await _context.SaveChangesAsync();
    }
    */
    /*
    public async Task UpdatePassword(string phoneNumber, string newPassword)
    {
        // Retrieve user entity by phone number
        var userEntity = await _context.Users
                             .FirstOrDefaultAsync(u => u.Phone == phoneNumber)
                         ?? throw new Exception("User not found");

        // Hash the new password using the injected IPasswordHasher
        userEntity.PasswordHash = _passwordHasher.Generate(newPassword);

        // Update the user entity in the context
        _context.Users.Update(userEntity);
        await _context.SaveChangesAsync();
    }
    */



}
