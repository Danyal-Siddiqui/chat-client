using ChatClient.Application.Services.Interfaces;
using ChatClient.Contracts.V1;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace ChatClient.Controllers;

[ApiController]
[Route("api/v1/chat-client")]
public class ChatClientController : ControllerBase
{

    private readonly ILogger _logger;
    private readonly IMessagingService _messagingService;

    public ChatClientController(ILogger logger, IMessagingService messagingService)
    {
        _logger = logger;
        _messagingService = messagingService;
    }

    [HttpPost(template:"publish-message")]
    public async Task<IActionResult> PublishMessage([FromBody] PublishMessage publishMessage)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(publishMessage.Message))
                throw new ArgumentException("Message can't be null, empty or whitespace");
            if (string.IsNullOrWhiteSpace(publishMessage.Username))
                throw new ArgumentException("Username can't be null, empty or whitespace");
            await _messagingService.PublishMessage(publishMessage.Message, publishMessage.Username);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.Error(e, e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost(template:"get-all-messages")]
    public async Task<IActionResult> GetAllMessages()
    {
        try
        {
            return Ok(await _messagingService.GetAllMessage());
        }
        catch (Exception e)
        {
            _logger.Error(e, e.Message);
            return BadRequest(e.Message);
        }
    }
}