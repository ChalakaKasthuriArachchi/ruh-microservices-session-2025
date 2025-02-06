using invoicing_service.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Thread.Sleep(10000);
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// RabbitMq
var rabbitMqConnection = new ConnectionFactory(){
            HostName = "rabbitmq",
            UserName = "guest",
            Password = "guest"
        }.CreateConnectionAsync().Result;

builder.Services.AddSingleton<IConnection>(rabbitMqConnection); 
builder.Services.AddSingleton<PaymentRequestService>(); 
builder.Services.AddHostedService<PaymentAckConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
