using ChatClient.Application.Services.Interfaces;
using ChatClient.Domain;
using Serilog;

namespace ChatClient.Application.Services;

public class MessagingService : IMessagingService
{
    private readonly ILogger _logger;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IMessageRepository _messageRepository;
    
    public MessagingService(ILogger logger, IMessagePublisher messagePublisher, IMessageRepository messageRepository)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
        _messageRepository = messageRepository;
    }
    
    public async Task PublishMessage(string message, string username)
    {
        await _messagePublisher.Publish(new Message(message, username));
    }
    
    public async Task MessageReceived(Message message)
    {
        _logger.Information("Message: {Message} received from user: {Username} which was published_at: {Timestamp}",
            message.Text,
            message.Username, 
            message.Timestamp);
        await _messageRepository.Add(message);
    }
    
    public async Task<IEnumerable<Message>> GetAllMessage() => await _messageRepository.GetAll();
}