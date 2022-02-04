using System.Text;
using System.Text.Json;
using ChatClient.Application.Services.Interfaces;
using ChatClient.Contracts.V1;
using ChatClient.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NATS.Client;

namespace ChatClient.Infrastructure.Messaging;

public class MessageSubscriber : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IServiceProvider _services;
    
    public MessageSubscriber(IConnection connection, IServiceProvider services)
    {
        _connection = connection;
        _services = services;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IAsyncSubscription s = _connection.SubscribeAsync("foo", MsgHandler);
        return Task.CompletedTask;
    }

    private void MsgHandler(object sender, MsgHandlerEventArgs args)
    {
        using var scope = _services.CreateScope();
        var messagingService = scope.ServiceProvider.GetRequiredService<IMessagingService>();
        var payload = JsonSerializer.Deserialize<MessagePayload>(Encoding.UTF8.GetString(args.Message.Data));
        messagingService.MessageReceived(new Message(payload.Message, payload.Username, payload.Timestamp)).Wait();
    }
}