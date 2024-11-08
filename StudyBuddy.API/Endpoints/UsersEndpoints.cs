using StudyBuddy.API.Contracts.Users;
using StudyBuddy.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace StudyBuddy.API.Endpoints;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("send-confirmation-code", SendConfirmationCode);
        app.MapPost("confirm-phone", VerifyConfirmationCode);
        app.MapPost("register", Register);
        app.MapPost("login", Login);

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
        UserService usersService)
    {
        var isCodeValid = usersService.VerifyConfirmationCode(request.PhoneNumber, request.ConfirmationCode);

        if (!isCodeValid)
        {
            return Results.BadRequest("Invalid confirmation code.");
        }

        // После подтверждения регистрация
        await usersService.Register(request.UserNickName, request.Password, request.UserFullName, request.PhoneNumber);
        
        return Results.Ok("Registration successful.");
    }
    
    private static async Task<IResult> Register(
        [FromBody] RegisterUserRequest request,
        UserService usersService)
    {
        await usersService.Register(request.UserPhone, request.UserNickName, request.UserFullName , request.Password);
        return Results.Ok();
    }
    
    /*
    private static async Task<IResult> ConfirmPhoneNumber(
        [FromBody] VerifyConfirmationCodeRequest request,
        UserService userService)
    {
        var savedCode = await userService.GetStoredConfirmationCode(request.PhoneNumber);

        if (savedCode == null)
        {
            return Results.BadRequest("No confirmation code found.");
        }

        if (request.Code != savedCode)
        {
            return Results.BadRequest("Invalid confirmation code.");
        }

        var user = await userService.GetByPhoneNumber(request.PhoneNumber);
        if (user != null)
        {
            return Results.BadRequest("Phone number already confirmed.");
        }


        return Results.Ok(new { Message = "Phone number confirmed" });
    }
    */

    private static async Task<IResult> Login(
        [FromBody] LoginUserRequest request,
        UserService usersService,
        HttpContext context)
    {
        var token = await usersService.Login(request.Phone, request.Password);

        context.Response.Cookies.Append("secretCookie", token);

        return Results.Ok(token);
    }
}