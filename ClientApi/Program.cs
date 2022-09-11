using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(x =>
{
    var srv = x.GetService<IConfiguration>();
    Debug.Assert(srv != null, nameof(srv) + " != null");
    var apiKey = srv["ApiKey"];
    var secretKey = srv["SecretKey"];
    if (apiKey == null || secretKey == null)
        throw new NullReferenceException("Incomplete credentials");
    return new TransactionSigner.TransactionSigner(apiKey, secretKey);
});
builder.Services.AddScoped(x =>
{
    var http = x.GetService<HttpClient>();
    if (http == null)
        throw new NullReferenceException($"No {nameof(HttpClient)} service registered");
    http.BaseAddress = new Uri("https://api.crypto.com/v2/");
    return http;
});

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
