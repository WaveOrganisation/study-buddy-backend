public class RegisterModel
{
    [Required]
    public string PhoneNumber { get; set; }
    
    [Required]
    public string UserNickName { get; set; }
    
    [Required]
    public string UserFullName { get; set; }
    
    [Required]
    public string Password { get; set; }
}

public class LoginModel
{
    [Required]
    public string PhoneNumber { get; set; }
    
    [Required]
    public string Password { get; set; }
}