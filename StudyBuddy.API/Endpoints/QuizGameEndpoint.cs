using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace StudyBuddy.API.Endpoints
{
    public static class QuizEndpoints
    {
        public static IEndpointRouteBuilder MapQuizEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("quiz/create", CreateGame);
            app.MapPost("quiz/join", JoinGame);
            app.MapPost("quiz/question", SendQuestion);
            app.MapPost("quiz/answer", SubmitAnswer);

            return app;
        }

        private static async Task<IResult> CreateGame(
            [FromBody] CreateGameRequest request,
            QuizService quizService,
            IHubContext<QuizHub> hubContext)
        {
            var result = await quizService.CreateGameAsync(request.GameId);
            if (!result.Success)
                return Results.BadRequest(result.ErrorMessage);

            await hubContext.Clients.All.SendAsync("GameCreated", request.GameId);
            return Results.Ok($"Game {request.GameId} created successfully.");
        }

        private static async Task<IResult> JoinGame(
            [FromBody] JoinGameRequest request,
            QuizService quizService,
            IHubContext<QuizHub> hubContext)
        {
            var result = await quizService.JoinGameAsync(request.GameId, request.PlayerName);
            if (!result.Success)
                return Results.BadRequest(result.ErrorMessage);

            await hubContext.Groups.AddToGroupAsync(request.ConnectionId, request.GameId);
            await hubContext.Clients.Group(request.GameId).SendAsync("PlayerJoined", request.PlayerName);
            return Results.Ok($"Player {request.PlayerName} joined game {request.GameId} successfully.");
        }

        private static async Task<IResult> SendQuestion(
            [FromBody] SendQuestionRequest request,
            QuizService quizService,
            IHubContext<QuizHub> hubContext)
        {
            var result = await quizService.SendQuestionAsync(request.GameId, request.Question);
            if (!result.Success)
                return Results.BadRequest(result.ErrorMessage);

            await hubContext.Clients.Group(request.GameId).SendAsync("NewQuestion", request.Question);
            return Results.Ok("Question sent successfully.");
        }

        private static async Task<IResult> SubmitAnswer(
            [FromBody] SubmitAnswerRequest request,
            QuizService quizService,
            IHubContext<QuizHub> hubContext)
        {
            var result = await quizService.SubmitAnswerAsync(request.GameId, request.PlayerName, request.Answer);
            if (!result.Success)
                return Results.BadRequest(result.ErrorMessage);

            await hubContext.Clients.Group(request.GameId).SendAsync("AnswerReceived", request.PlayerName, request.Answer);
            return Results.Ok("Answer submitted successfully.");
        }
    }

    // Перавй тест 1Д
    public record CreateGameRequest(string GameId);
    public record JoinGameRequest(string GameId, string PlayerName, string ConnectionId);
    public record SendQuestionRequest(string GameId, string Question);
    public record SubmitAnswerRequest(string GameId, string PlayerName, string Answer);
}

public class QuizService
{
    // Метод создания игры
    public async Task<Result> CreateGameAsync(string gameId)
    {
        // Логика создания игры
        return new Result { Success = true };
    }

    // Метод присоединения к игре
    public async Task<Result> JoinGameAsync(string gameId, string playerName)
    {
        // Логика присоединения к игре
        return new Result { Success = true };
    }

    // Метод отправки вопроса
    public async Task<Result> SendQuestionAsync(string gameId, string question)
    {
        // Логика отправки вопроса
        return new Result { Success = true };
    }

    // Метод отправки ответа
    public async Task<Result> SubmitAnswerAsync(string gameId, string playerName, string answer)
    {
        // Логика обработки ответа
        return new Result { Success = true };
    }
}

public class Result
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
}