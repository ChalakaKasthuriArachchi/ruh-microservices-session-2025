using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using invoicing_service.Messages;
using invoicing_service.Models;

public class PaymentAckConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private const string PAYMENT_ACK_QUEUE = "payment-ack";
    private readonly IServiceProvider _serviceProvider;

    public PaymentAckConsumer(IConnection connection, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _connection = connection;
        _channel = connection.CreateChannelAsync().Result;

        _channel.QueueDeclareAsync(PAYMENT_ACK_QUEUE, durable: true, exclusive: false, autoDelete: false, arguments: null).Wait();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received: {json}");

            var ack = JsonSerializer.Deserialize<PaymentAck>(json);

            UpdateInvoice(ack!);

            return Task.CompletedTask;
        };
        return _channel.BasicConsumeAsync(queue: PAYMENT_ACK_QUEUE, autoAck: true, consumer: consumer);
    }

    private void UpdateInvoice(PaymentAck ack)
    {
        Console.WriteLine($"Payment Ack Received. Invoice Id: {ack?.InvoiceId}, Payment Status: {ack?.PaymentStatus}");

        using (var scope = _serviceProvider.CreateScope())
        {
            // Resolve the DbContext within the scope
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Use the DbContext
            var invoice = dbContext?.Invoices?.FirstOrDefault(inv => inv.Id == ack!.InvoiceId);
            if(invoice != null)
            {
                invoice!.PaymentStatus = ack!.PaymentStatus;
                dbContext!.SaveChanges();
            }
            
        }
    }

    public override void Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
        base.Dispose();
    }
}
