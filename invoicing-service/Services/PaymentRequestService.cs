using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

public class PaymentRequestService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private const string PAYMENT_REQUEST_QUEUE = "payment-request";

    public PaymentRequestService(IConnection connection)
    {
        _connection = connection;
        _channel = connection.CreateChannelAsync().Result;

        _channel.QueueDeclareAsync(PAYMENT_REQUEST_QUEUE, durable: true, exclusive: false, autoDelete: false, arguments: null).Wait();
    }

    public async Task PublishMessage<T>(T message)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.BasicPublishAsync(exchange: "", routingKey: PAYMENT_REQUEST_QUEUE, body: body);
    }

    public void Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
    }
}
