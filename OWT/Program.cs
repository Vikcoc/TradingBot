using System.Data;
using System.Data.SqlClient;
using OWT.BackgroundService;
using OWT.CryptoCom;
using OWT.CryptoCom.ResponseHandlers;
using OWT.SocketClient;
using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDbConnection>(db => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<CryptoComMarketClient>();
builder.Services.AddTransient<ISocketClient, SocketClient>();
builder.Services.AddTransient<ClientWebSocket>();
builder.Services.AddScoped<HeartbeatHandler>();
//builder.Services.AddScoped<TickerHandler>();
builder.Services.AddScoped<TickerSaveHandler>();
builder.Services.AddScoped<CryptoComDtoDecider>(s =>
{
    var res = new CryptoComDtoDecider();
    res.AddHandler(s.GetRequiredService<HeartbeatHandler>());
    //res.AddHandler(s.GetRequiredService<TickerHandler>());
    res.AddHandler(s.GetRequiredService<TickerSaveHandler>());
    return res;
});
builder.Services.AddHostedService<CryptoComDataCollector>();
//builder.Services.AddSingleton<CryptoComDataCollector>(s => s.GetRequiredService<CryptoComDataCollector>());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
