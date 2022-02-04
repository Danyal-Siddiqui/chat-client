# chat-client-service

## Steps to setup application:
1. Install Visual Studio 2022/ Latest Rider
2. Install .NET 6 sdk
3. Have a running Nats server
4. Update the Nats server address in src/ChatClient/Program.cs file line no 27 (Right now it's set to default "nats://localhost:4222")

## Steps to use the application:
1: Go to http://localhost:5000/swagger/index.html
2: To publish message along with your username use this endpoint ```api/v1/chat-client/publish-message```
3: To get all the publish message you can use this endpoint ```api/v1/chat-client/get-all-messages```