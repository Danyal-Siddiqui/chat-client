using ChatClient.Application.Services;
using ChatClient.Application.Services.Interfaces;
using ChatClient.Infrastructure.Messaging;
using ChatClient.Infrastructure.Repositories;
using NATS.Client;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
    builder.Services.AddTransient<IMessagePublisher, MessagePublisher>();
    builder.Services.AddTransient<IMessagingService, MessagingService>();
    builder.Services.AddSingleton<IMessageRepository, MessageRepository>();
    
    builder.Services.AddHostedService<MessageSubscriber>();
    
    builder.Services.AddNatsClient();
    builder.Services.AddNatsClient(connectionServiceLifeTime: ServiceLifetime.Singleton);
    builder.Services.AddNatsClient(configureOptions: options =>
    {
        options.Servers = new[]
        {
            "nats://localhost:4222"
        };

        options.MaxReconnect = 2;
        options.ReconnectWait = 1000;
    });
    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run("http://*:5000");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}