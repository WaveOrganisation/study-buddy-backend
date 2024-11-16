// API для отправки кода и подтверждения номера
using StudyBuddy.API.Contracts.Users;
using StudyBuddy.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace StudyBuddy.API.Endpoints;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("auth/send-confirmation-code", SendConfirmationCode);
        app.MapPost("auth/confirm-phone", VerifyConfirmationCode);
        app.MapPost("auth/register", Register);
        app.MapPost("auth/login", Login);

        return app;
    }

    private static async Task<IResult> SendConfirmationCode(
        [FromBody] SendConfirmationCodeRequest request,
        UserService userService)
    {
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
}
