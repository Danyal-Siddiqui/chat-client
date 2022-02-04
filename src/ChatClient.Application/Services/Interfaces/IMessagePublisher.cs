using ChatClient.Domain;

namespace ChatClient.Application.Services.Interfaces;

public interface IMessagePublisher
{
    Task Publish(Message message);
}