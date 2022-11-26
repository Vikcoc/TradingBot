using System.Diagnostics;
using Traders.CryptoCom;
using Traders.CryptoCom.Socket;
using WebSocketFlow.SocketAdapter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//my socket
builder.Services.AddTransient<ISocketAdapter, SocketAdapter>();
builder.Services.AddSingleton<IMarketAdapter, MarketAdapter>();
builder.Services.AddSingleton<IUserAdapter, UserAdapter>(x =>
{
    var configuration = x.GetService<IConfiguration>();
    Debug.Assert(configuration != null, nameof(configuration) + " != null");
    var apiKey = configuration["ApiKey"];
    var secretKey = configuration["SecretKey"];
    Debug.Assert(apiKey != null, nameof(apiKey) + " != null");
    Debug.Assert(secretKey != null, nameof(secretKey) + " != null");
    var socketAdapter = x.GetService<ISocketAdapter>();
    Debug.Assert(socketAdapter != null, nameof(socketAdapter) + " != null");
    return new UserAdapter(socketAdapter, apiKey, secretKey);
});

builder.Services.AddTransient<TradingWebSocket.Socket.ISocketAdapter, TradingWebSocket.Socket.SocketAdapter>();
builder.Services.AddSingleton<CryptoComMarketAdapter>();
builder.Services.AddSingleton(x =>
{
    var configuration = x.GetService<IConfiguration>();
    Debug.Assert(configuration != null, nameof(configuration) + " != null");
    var apiKey = configuration["ApiKey"];
    var secretKey = configuration["SecretKey"];
    Debug.Assert(apiKey != null, nameof(apiKey) + " != null");
    Debug.Assert(secretKey != null, nameof(secretKey) + " != null");
    var socketAdapter = x.GetService<TradingWebSocket.Socket.ISocketAdapter>();
    Debug.Assert(socketAdapter != null, nameof(socketAdapter) + " != null");
    return new CryptoComUserAdapter(socketAdapter, apiKey, secretKey);
});
builder.Services.AddSingleton<CryptoComTrader>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
