using Microsoft.Extensions.DependencyInjection;
using Patients.EventBus.Abstraction;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;

namespace Patients.EventBus.Rabbit;

public class RabbitMQEventBus : IPOEventBus
{
    private readonly string _hostname;
    private readonly string _exchangeName;
    private readonly IConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQEventBus(IConnectionFactory factory , IOptions<RabbitMQSettings> options)
    {
        _factory = factory;
        var settings = options.Value;
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        _exchangeName = settings.ExchangeName;
        _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout);
    }

    public async Task Publish<T>(T messageToPublish) where T : IntegrationMsg
    {
        string message = JsonSerializer.Serialize(messageToPublish);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(
            exchange: _exchangeName,
            routingKey: "",
            basicProperties: null,
            body: body
        );

        await Task.CompletedTask; 
    }

    public async Task Subscribe<T>(Action<T> callBack) where T : IntegrationMsg
    {
        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: "");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var integrationMsg = JsonSerializer.Deserialize<T>(message);
            callBack(integrationMsg);
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        await Task.CompletedTask; // Subscribing is also synchronous in RabbitMQ
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}


public static class RabbitEx
{
    public static void AddRabbit(this IServiceCollection collection)
    {
        collection.AddSingleton<IPOEventBus, RabbitMQEventBus>();
    }
}
