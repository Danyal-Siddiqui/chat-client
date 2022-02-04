using ChatClient.Domain;

namespace ChatClient.Application.Services.Interfaces;

public interface IMessageRepository
{
    Task Add(Message message);
    Task<IEnumerable<Message>> GetAll();
}