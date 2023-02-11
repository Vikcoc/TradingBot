using OWT.BackgroundService;
using OWT.CryptoCom;
using OWT.SocketClient;
using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<CryptoComMarketClient>();
builder.Services.AddTransient<ISocketClient, SocketClient>();
builder.Services.AddTransient<ClientWebSocket>();
builder.Services.AddHostedService<CryptoComDataCollector>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
