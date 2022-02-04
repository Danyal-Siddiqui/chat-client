namespace ChatClient.Domain;

public class Message
{
    public Message(string text, string username, DateTime? timestamp = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Message text can't be null, empty or whitespace");
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username can't be null, empty or whitespace");
        
        Text = text;
        Username = username;
        Timestamp = timestamp ?? DateTime.UtcNow;
    }
    
    public string Text { get; }
    public string Username { get; }
    public DateTime Timestamp { get; }
}