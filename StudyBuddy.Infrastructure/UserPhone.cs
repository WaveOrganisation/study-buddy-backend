using StudyBuddy.Application.Interfaces.Auth;
using StudyBuddy.Application.Services;

namespace StudyBuddy.Infrastructure;

public class UserPhone : IUserPhone
{
    
    
    public async bool VerifyPhoneNumber(string phoneNumber, string verificationCode)
    {
        var smsService = new SmsService();
        
        await smsService.SendSmsAsync(phoneNumber, verificationCode);
        
        return phoneNumber == verificationCode;
    }
}