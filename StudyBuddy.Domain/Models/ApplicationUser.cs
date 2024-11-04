using System.ComponentModel.DataAnnotations;

public class ApplicationUser : IdentityUser
{
    public string UserFullName { get; set; }
    public string UserNickName { get; set; }
}
