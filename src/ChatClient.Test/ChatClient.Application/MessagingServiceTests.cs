using System;
using System.Threading.Tasks;
using ChatClient.Application.Services;
using ChatClient.Application.Services.Interfaces;
using ChatClient.Domain;
using FakeItEasy;
using Serilog;
using Xunit;

namespace ChatClient.Test.ChatClient.Application;

public class MessagingServiceTests
{
    private readonly ILogger _logger;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IMessageRepository _messageRepository;
    private readonly MessagingService _sut;

    public MessagingServiceTests()
    {
        _logger = A.Fake<ILogger>();  
        _messagePublisher = A.Fake<IMessagePublisher>();  
        _messageRepository = A.Fake<IMessageRepository>();
        _sut = new MessagingService(_logger, _messagePublisher, _messageRepository);
    }

    [Theory]
    [InlineData("", "someUser")]
    [InlineData(" ", "someUser")]
    [InlineData(null, "someUser")]
    [InlineData("someMessage", "")]
    [InlineData("someMessage", " ")]
    [InlineData("someMessage", null)]
    public async Task PublishMessage_InvalidParameters_ThrowArgumentException(string message, string username)
    {
        Task Action() => _ = _sut.PublishMessage(message, username);
        await Assert.ThrowsAsync<ArgumentException>(Action);
    }
    
    [Fact]
    public async Task PublishMessage_ValidParameters_PublishMessage()
    {
        var message = "someMessage";
        var username = "someUser";
        await _sut.PublishMessage(message, username);
        A.CallTo(() => _messagePublisher.Publish(A<Message>._))
            .MustHaveHappened();
    }
    
    [Fact]
    public async Task MessageReceived_ValidMessageReceived_LogAndSaveMessage()
    {
        var message = new Message("someMessage", "someUser");
        await _sut.MessageReceived(message);
        A.CallTo(() => _logger.Information("Message: {Message} received from user: {Username} which was published_at: {Timestamp}",
                message.Text,
                message.Username, 
                message.Timestamp))
            .MustHaveHappened();
        A.CallTo(() => _messageRepository.Add(A<Message>._))
            .MustHaveHappened();
    }
    
}