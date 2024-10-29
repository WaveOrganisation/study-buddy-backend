using StudyBuddy.API.Contracts.Users;
using StudyBuddy.Application.Services;

namespace StudyBuddy.API.Endpoints;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("register", Register);
        app.MapPost("login", Login);
        
        return app;
    }

    private static async Task<IResult> Register(
        RegisterUserRequest request,UsersService usersService, ILogger logger)
    {
        await usersService.Register(request.);
        
        return Results.Ok("Register");
    }

    private static async Task<IResult> Login()
    {
        return Results.Ok("Login");
    }
}