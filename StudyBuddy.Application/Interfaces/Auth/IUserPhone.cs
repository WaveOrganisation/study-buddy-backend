namespace StudyBuddy.Application.Interfaces.Auth;

public interface IUserPhone
{
    bool VerifyPhoneNumber(string UserPhone, string VerificationCode);
}