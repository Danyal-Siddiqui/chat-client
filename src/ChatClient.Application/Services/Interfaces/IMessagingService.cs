using ChatClient.Domain;

namespace ChatClient.Application.Services.Interfaces;

public interface IMessagingService
{
    Task PublishMessage(string message, string username);
    Task MessageReceived(Message message);
    Task<IEnumerable<Message>> GetAllMessage();
}