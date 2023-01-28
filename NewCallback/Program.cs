using NewCallback.EndpointDefinitions;
using NewCallback.TestingClasses;
using System.Diagnostics;
using System.Net.WebSockets;
using WebSocketService;
using WebSocketService.DefaultImplementations;
using WebSocketService.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Services
//todo https://learn.microsoft.com/en-us/dotnet/core/extensions/scoped-service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ClientWebSocket>();
builder.Services.AddScoped<INotificationConsumer<INotification<string>, string>[]>(provider =>
{
    var loggerFactory = provider.GetService<ILoggerFactory>();
    Debug.Assert(loggerFactory != null);
    var scopedGuid = provider.GetService<ScopedGuid>();
    Debug.Assert(scopedGuid != null);
    return new INotificationConsumer<INotification<string>, string>[]
    {
        new SocketMessagesConsumer(loggerFactory.CreateLogger<SocketMessagesConsumer>(), scopedGuid),
        //new SocketMessagesConsumer2(loggerFactory.CreateLogger<SocketMessagesConsumer2>(), scopedGuid)
    };
});
builder.Services.AddScoped<INotificationConsumer<INotification<string>, string>, SocketMessagesConsumer2>();
builder.Services.AddScoped<INotification<string>, Notification<string>>();
builder.Services.AddScoped<ScopedGuid>();
builder.Services.AddHostedService<Worker>();

#endregion

var app = builder.Build();

#region Endpoints

app.UseSwagger();

app.DefineClientTestEndpoints();

app.UseSwaggerUI();

#endregion

app.Run();
