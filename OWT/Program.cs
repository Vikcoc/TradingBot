using System.Data;
using System.Data.SqlClient;
using System.Net.WebSockets;
using OWT.CryptoCom;
using OWT.CryptoCom.BackgroundService;
using OWT.CryptoCom.Deciders;
using OWT.CryptoCom.Dto;
using OWT.CryptoCom.ResponseHandlers;
using OWT.SocketClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDbConnection>(db =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<CryptoComMarketClient>();
builder.Services.AddTransient<CryptoComUserClient>();
builder.Services.AddTransient<CryptoComCredentials>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new CryptoComCredentials(configuration["ApiKey"] ?? "", configuration["SecretKey"] ?? "");
});
builder.Services.AddTransient<ISocketClient, SocketClient>();
builder.Services.AddTransient<ClientWebSocket>();
builder.Services.AddScoped<HeartbeatHandler>();
builder.Services.AddScoped<TickerSaveHandler>();
builder.Services.AddScoped<TickerWithEstimationHandler>();
builder.Services.AddScoped<BalanceHandler>();
builder.Services.AddScoped<CryptoComBalanceDto>();
builder.Services.AddScoped<SecondPass>();
builder.Services.AddScoped<TradingEthHandler9>();
builder.Services.AddScoped<OrderLoggingHandler>();
builder.Services.AddScoped<TickerSaveHandler2>();
builder.Services.AddScoped<CryptoComPurchaseDecider>();
builder.Services.AddScoped(s =>
{
    var res = new CryptoComMarketDtoDecider();
    res.AddHandler(s.GetRequiredService<HeartbeatHandler>());
    res.AddHandler(s.GetRequiredService<TickerSaveHandler>());
    res.AddHandler(s.GetRequiredService<TickerSaveHandler2>());
    return res;
});
builder.Services.AddScoped(s =>
{
    var res = new CryptoComUserDtoDecider();
    res.AddHandler(s.GetRequiredService<HeartbeatHandler>());
    res.AddHandler(s.GetRequiredService<BalanceHandler>());
    res.AddHandler(s.GetRequiredService<TradingEthHandler9>());
    res.AddHandler(s.GetRequiredService<OrderLoggingHandler>());
    return res;
});
//builder.Services.AddHostedService<CryptoComDataCollector>();
builder.Services.AddHostedService<CryptoComUserCollector>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();