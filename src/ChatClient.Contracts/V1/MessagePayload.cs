namespace ChatClient.Contracts.V1;

public record MessagePayload(string Message, string Username, DateTime Timestamp);