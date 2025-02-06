using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Thread.Sleep(5000);
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// RabbitMq
var rabbitMqConnection = new ConnectionFactory(){
            HostName = "rabbitmq",
            UserName = "guest",
            Password = "guest"
        }.CreateConnectionAsync().Result;

builder.Services.AddSingleton<IConnection>(rabbitMqConnection); 
builder.Services.AddSingleton<PaymentAckService>(); 
builder.Services.AddHostedService<PaymentRequestConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
