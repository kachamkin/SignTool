using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using static WebSignTool.ChatHub;

namespace WebSignTool
{
    public interface IWebSocket
    {
        public event MessageReceived? OnMessageReceived;
        public Task SendMessage(string message);
    }

    public class ChatHub : Hub, IWebSocket
    {
        private readonly HubConnection connection;
        private readonly ITelegram telegram;
        private readonly IConfiguration configuration;
        public delegate void MessageReceived(string message, ITelegram telegram);
        public event MessageReceived? OnMessageReceived;

        public ChatHub(IConfiguration _config, ITelegram _telegram)
        {
            configuration = _config;
            telegram = _telegram;
            
            connection = new HubConnectionBuilder().WithUrl(configuration.GetConnectionString("WebSocket")).Build();
            connection.On<string>("Receive", (message) => OnMessageReceived?.Invoke(message, telegram));
            connection.StartAsync();
        }

        public async Task SendMessage(string message)
        {
            await connection.InvokeAsync("Forward", message);
        }
        
        public async Task Forward(string message)
        {
            await this.Clients.All.SendAsync("Receive", message); 
        }
    }
}
