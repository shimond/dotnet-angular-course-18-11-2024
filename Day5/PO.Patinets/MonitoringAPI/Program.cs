using MonitoringAPI.Apis;
using MonitoringAPI.Contracts;
using MonitoringAPI.Services;
using StackExchange.Redis;
using Patients.EventBus.Rabbit;
using Microsoft.AspNetCore.Builder;
using MonitoringAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("Rabbit"));

builder.Services.AddSingleton<IConnectionMultiplexer>(
    _ => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("myRedis"))
);

builder.Services.AddCors(x => x.AddDefaultPolicy(
    p =>
        p.AllowAnyMethod()
        .AllowCredentials()
        .AllowAnyHeader()
        .SetIsOriginAllowed(_ => true)));
    
builder.Services.AddSignalR();
builder.Services.AddScoped<IMonitorService, MonitorService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRabbit();

var app = builder.Build();
app.UseCors();
app.Use(async (context, next) => {
    await next(); // for debug
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<MonitorHub>("/monitorHub");

app.UseMonitorApi();
app.Run();
