using static WebSignTool.Telegram;

namespace WebSignTool
{
    public interface ITelegram
    {
        public event MessageReceived? OnMessageReceived;
        public void SendMessage(string message);
        public int UpdateId { get; }

    }

    public class From
    {
        public int id { get; set; } = 0;
        public bool is_bot { get; set; } = false;
        public string first_name { get; set; } = "";
        public string last_name { get; set; } = "";
        public string username { get; set; } = "";
        public string language_code { get; set; } = "";
    }

    public class Chat
    {
        public int id { get; set; } = 0;
        public string first_name { get; set; } = "";
        public string last_name { get; set; } = "";
        public string username { get; set; } = "";
        public string type { get; set; } = "";
    }

    public class Message
    {
        public int message_id { get; set; } = 0;
        public From from { get; set; } = new();
        public Chat chat { get; set; } = new();
        public int date { get; set; } = 0;
        public string text { get; set; } = "";
    }

    public class Result
    {
        public int update_id { get; set; } = 0;
        public Message message { get; set; } = new();
    }

    public class LastMessage
    {
        public bool ok { get; set; } = false;
        public List<Result> result { get; set; } = new();
    }

    public class Telegram : ITelegram
    {
        private readonly string botToken;
        private readonly int chatId;
        private readonly uint checkNewMessagesIntervalInSeconds;
        private int updateId;
        private readonly System.Timers.Timer timer;
        public int UpdateId { get { return updateId; } }

        public delegate void MessageReceived(string message, ITelegram telegram);
        public event MessageReceived? OnMessageReceived;

        public Telegram(string _botToken, int _chatId, int _updateId, uint _checkNewMessagesIntervalInSeconds = 1)
        {
            botToken = _botToken;  
            chatId = _chatId;
            updateId = _updateId;
            checkNewMessagesIntervalInSeconds = _checkNewMessagesIntervalInSeconds;

            timer = new(1000 * checkNewMessagesIntervalInSeconds);
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        ~Telegram()
        {
            timer.Stop();
            timer.Dispose();
        }

        private async void TimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (OnMessageReceived != null)
            {
                OnMessageReceived.Invoke(await GetLastMessage(), this);
            }
        }

        public async void SendMessage(string message)
        {
            using HttpClient hc = new();
            await hc.GetStringAsync(
                                "https://api.telegram.org/bot" +
                                botToken +
                                "/sendMessage?chat_id=" +
                                chatId +
                                "&text=" + message
                             );
        }

        private async Task<string> GetLastMessage()
        {
            using HttpClient hc = new();

            try
            {
                LastMessage? lastMessage = await hc.GetFromJsonAsync<LastMessage>(
                                                    "https://api.telegram.org/bot" +
                                                    botToken +
                                                    "/getUpdates?offset=-1"
                                                );
                if (lastMessage == null || !lastMessage.ok || lastMessage.result.Count == 0 || lastMessage.result[0].update_id == updateId || lastMessage.result[0].message.from.id != chatId)
                    return "";

                updateId = lastMessage.result[0].update_id;
                return lastMessage.result[0].message.text;
            }
            catch
            {
                return "";
            }
        }
    }
}
