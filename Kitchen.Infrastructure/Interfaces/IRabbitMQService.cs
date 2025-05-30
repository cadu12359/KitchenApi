namespace Kitchen.Infrastructure.Interfaces;

public interface IRabbitMQService
{
    void PublishMessage(string queueName, object message);
}