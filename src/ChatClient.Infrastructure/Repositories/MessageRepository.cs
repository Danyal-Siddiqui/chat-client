using ChatClient.Application.Services.Interfaces;
using ChatClient.Domain;

namespace ChatClient.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private List<Message> _receivedMessages;

    public MessageRepository()
    {
        _receivedMessages = new List<Message>();
    }
    
    public async Task Add(Message message) => _receivedMessages.Add(message);

    public async Task<IEnumerable<Message>> GetAll() => _receivedMessages;
}