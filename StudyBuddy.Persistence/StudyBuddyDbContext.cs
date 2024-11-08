using StudyBuddy.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace StudyBuddy.Persistence;

public class StudyBuddyDbContext(DbContextOptions<StudyBuddyDbContext> options)
    : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}