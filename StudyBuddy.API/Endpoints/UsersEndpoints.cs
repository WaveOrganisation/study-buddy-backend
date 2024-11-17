// API для отправки кода и подтверждения номера
using StudyBuddy.API.Contracts.Users;
using StudyBuddy.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace StudyBuddy.API.Endpoints;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/test", TestMap);
        
        app.MapPost("auth/send-confirmation-code", SendConfirmationCode);
        app.MapPost("auth/confirm-phone", VerifyConfirmationCode);
        
        app.MapPost("auth/register", Register);
        app.MapPost("auth/login", Login);

        app.MapPost("auth/forgot-password", ForgotPassword);
        app.MapPost("auth/reset-password", ResetPassword);

        
        return app;
    }

    private static async Task<IResult> TestMap()
    {
        return Results.Ok();
    }
    
    private static async Task<IResult> SendConfirmationCode(
        [FromBody] SendConfirmationCodeRequest request,
        UserService userService)
    {
        
        if(await userService.IsPhoneNumberTaken(request.PhoneNumber))
            return Results.BadRequest("Phone number is already in use.");
        
        var code = userService.GenerateConfirmationCode(request.PhoneNumber);
        return Results.Ok(new { ConfirmationCode = code });
    }

    private static async Task<IResult> VerifyConfirmationCode(
        [FromBody] VerifyConfirmationCodeRequest request,
        UserService userService)
    {
        var isCodeValid = userService.VerifyConfirmationCode(request.PhoneNumber, request.ConfirmationCode);

        if (!isCodeValid)
        {
            return Results.BadRequest("Invalid confirmation code.");
        }

        // Устанавливаем подтверждение для номера телефона
        userService.MarkPhoneAsConfirmed(request.PhoneNumber);
        return Results.Ok("Phone confirmed successfully.");
    }

    private static async Task<IResult> Register(
        [FromBody] RegisterUserRequest request,
        UserService userService)
    {
        var isPhoneConfirmed = userService.IsPhoneConfirmed(request.PhoneNumber);

        if (!isPhoneConfirmed)
        {
            return Results.BadRequest("Phone number not confirmed.");
        }

        await userService.Register(request.UserNickName, request.Password, request.UserFullName, request.PhoneNumber);
        return Results.Ok("Registration successful.");
    }

    private static async Task<IResult> Login(
        [FromBody] LoginUserRequest request,
        UserService userService,
        HttpContext context)
    {
        var token = await userService.Login(request.Phone, request.Password);
        context.Response.Cookies.Append("secretCookie", token);
        return Results.Ok(token);
    }
    
    private static async Task<IResult> ForgotPassword(
        [FromBody] ForgotPasswordRequest request,
        UserService userService)
    {
        if (!await userService.IsPhoneNumberTaken(request.PhoneNumber))
        {
            return Results.BadRequest("Phone number is not registered.");
        }

        var code = userService.GenerateResetPasswordCode(request.PhoneNumber);
        return Results.Ok(new { ResetCode = code });
    }
    
    private static async Task<IResult> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        UserService userService)
    {
        var isCodeValid = userService.VerifyResetPasswordCode(request.PhoneNumber, request.ResetCode);

        if (!isCodeValid)
        {
            return Results.BadRequest("Invalid reset code.");
        }

        await userService.UpdatePassword(request.PhoneNumber, request.NewPassword);
        return Results.Ok("Password has been reset successfully.");
    }


}
