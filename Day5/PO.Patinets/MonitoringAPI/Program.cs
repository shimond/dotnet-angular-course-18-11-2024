using MonitoringAPI.Apis;
using MonitoringAPI.Contracts;
using MonitoringAPI.Services;
using StackExchange.Redis;
using Patients.EventBus.Rabbit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("Rabbit"));

builder.Services.AddSingleton<IConnectionMultiplexer>(
    _ => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("myRedis"))
);

builder.Services.AddScoped<IMonitorService, MonitorService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRabbit();
var app = builder.Build();

app.Use(async (context, next) => {
    await next(); // for debug
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMonitorApi();
app.Run();
