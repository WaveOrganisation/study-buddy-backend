namespace StudyBuddy.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public int UserNumber { get; set; }
    public string UserNickName { get; set; }
    public string UserFullName { get; set; }
    public string HashPassword { get; set; }
}


