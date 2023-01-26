using MediatR;
using NewCallback.EndpointDefinitions;
using WebSocketService;
using WebSocketService.DefaultImplementations;
using WebSocketService.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddTransient<IStringNotificationBuilder, StringNotificationBuilder>();
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
