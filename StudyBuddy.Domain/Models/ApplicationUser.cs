using Microsoft.AspNetCore.Identity;

namespace StudyBuddy.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; } = "Student"; 
    }
}
