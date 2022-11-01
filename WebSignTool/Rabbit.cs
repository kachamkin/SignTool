using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WebSignTool
{
    public interface IRabbit
    {
        public void SendMessage(string message);
    }

    public class Rabbit : BackgroundService, IRabbit
    {
        private readonly IConfiguration configuration;
        private readonly IModel channel;
        private EventingBasicConsumer? consumer;
        private readonly IConnection connection;
        private readonly ITelegram telegram;
        public Rabbit(IConfiguration _config, ITelegram _telegram)
        {
            configuration = _config;
            telegram = _telegram;

            ConnectionFactory factory = new() { Uri = new(configuration.GetConnectionString("Rabbit")) };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(configuration.GetSection("Options").GetValue<string>("RabbitQueue"), true, false, false);
        }
        ~Rabbit()
        {
            channel.Dispose();
            connection.Dispose();
        }

        public delegate void MessageReceived(string message, ITelegram telegram);  
        public event MessageReceived? OnMessageReceived;

        public void SendMessage(string message) =>
            channel.BasicPublish("", configuration.GetSection("Options").GetValue<string>("RabbitQueue"), null, new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(message)));

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                consumer = new(channel);
                consumer.Received += (model, ea) =>
                    OnMessageReceived?.Invoke(Encoding.UTF8.GetString(ea.Body.ToArray()), telegram);

                channel.BasicConsume(configuration.GetSection("Options").GetValue<string>("RabbitQueue"), true, consumer);
            }

            return Task.CompletedTask;
        }
    }
}
