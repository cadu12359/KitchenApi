using System.Text;
using System.Text.Json;
using Kitchen.Infrastructure.Interfaces;
using RabbitMQ.Client;

namespace Kitchen.Infrastructure.Services;

public class RabbitMQService : IDisposable, IRabbitMQService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    
    public RabbitMQService(IConnection connection, IModel channel)
    {
        _connection = connection;
        _channel = channel;
    }

    public RabbitMQService(string hostName, string userName, string password)
    {
        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            UserName = userName,
            Password = password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declara as filas
        _channel.QueueDeclare(queue: "fritos_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueDeclare(queue: "grelhados_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueDeclare(queue: "saladas_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueDeclare(queue: "bebidas_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueDeclare(queue: "sobremesa_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void PublishMessage(string queueName, object message)
    {
        if (_channel == null)
        {
            throw new InvalidOperationException("Canal do RabbitMQ não foi inicializado.");
        }

        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}