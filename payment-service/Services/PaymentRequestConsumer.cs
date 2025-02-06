using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using payment_service.Messages;

public class PaymentRequestConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private const string PAYMENT_REQUEST_QUEUE = "payment-request";
    private readonly PaymentAckService _paymentAckService;

    public PaymentRequestConsumer(IConnection connection, PaymentAckService paymentackService)
    {
        _connection = connection;
        _paymentAckService = paymentackService;
        _channel = connection.CreateChannelAsync().Result;

        _channel.QueueDeclareAsync(PAYMENT_REQUEST_QUEUE, durable: true, exclusive: false, autoDelete: false, arguments: null).Wait();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var request = JsonSerializer.Deserialize<PaymentRequest>(json);

            Console.WriteLine($"Payment Request Received. Invoice Id: {request?.InvoiceId}, Amount: {request?.TotalAmount}");

            Thread.Sleep(5000);

            var paymentAck = new PaymentAck(){
                InvoiceId = request!.InvoiceId,
                PaymentStatus = "SUCCESS"
            };
            return _paymentAckService.PublishMessage(paymentAck);
        };
        return _channel.BasicConsumeAsync(queue: PAYMENT_REQUEST_QUEUE, autoAck: true, consumer: consumer);
    }

    public override void Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
        base.Dispose();
    }
}
