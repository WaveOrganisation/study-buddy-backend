using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class QuizHub : Hub
{
    public async Task JoinGame(string gameId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        await Clients.Group(gameId).SendAsync("ReceiveMessage", $"{Context.ConnectionId} присоединился к игре.");
    }

    public async Task SendQuestionToGroup(string gameId, string question)
    {
        await Clients.Group(gameId).SendAsync("ReceiveQuestion", question);
    }

    public async Task SendAnswer(string gameId, string answer)
    {
        await Clients.Group(gameId).SendAsync("ReceiveAnswer", $"{Context.ConnectionId} отвечает: {answer}");
    }
}