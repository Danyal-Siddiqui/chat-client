using System.Text;
using System.Text.Json;
using ChatClient.Application.Services.Interfaces;
using ChatClient.Contracts.V1;
using ChatClient.Domain;
using NATS.Client;
using Serilog;

namespace ChatClient.Infrastructure.Messaging;

public class MessagePublisher : IMessagePublisher
{
    private readonly IConnection _connection;
    private readonly ILogger _logger;
    
    public MessagePublisher(IConnection connection, ILogger logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task Publish(Message message)
    {
        try
        {
            _connection.Publish(
                "foo",
                Encoding.UTF8.GetBytes(
                    JsonSerializer.Serialize(new MessagePayload(message.Text, message.Username, message.Timestamp))));
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error occur while publishing payload: {@Payload}", message);
            throw;
        }
    }
}