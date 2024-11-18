using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyApiAndMore.HostedService;
using MyApiAndMore.Models.Config;
using MyApiAndMore.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("smtp"));
builder.Services.AddScoped<ITimeManager, ServerUTCManager>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddTransient<IUsers, UsersManager>();
builder.Services.AddHostedService<SendServiceInEachMinute>();

//builder.Services.AddSingleton<ITimeManager, ServerUTCManager>();


var app = builder.Build();



app.Use(async (conetxt, next) => {
    app.Logger.LogInformation("Started");
    await next();
    app.Logger.LogInformation("Finished");

}); 

//app.Services.GetRequiredService<ITimeManager>();
app.MapGet("/time", (ITimeManager manager, IUsers usersManager) =>
{
    app.Logger.LogInformation("time started");
    var us = usersManager.GetCurrentUserName();
    return Results.Ok(manager.GetTime() + us);
});



app.MapGet("/sendEmailViaService", async (IEmailSender emailSender) => {
    var hostName = await emailSender.SendEmail();
    return Results.Ok(hostName);
});

app.MapGet("/sendEmail", (IOptionsSnapshot<SmtpConfig> config) => {
    return Results.Ok(config.Value);
});

app.MapGet("/sendEmailWithMonitor", (IOptionsMonitor<SmtpConfig> config) => {
    config.OnChange((x, s) => { 
        //x.HostName
    });
    return Results.Ok(config.CurrentValue);
});


app.MapGet("/config", (IConfiguration configuration) => {
    //https://source.dot.net/#Microsoft.AspNetCore/WebApplicationBuilder.cs,25a352b50e81d95b
    var c = configuration["smtp:Password"];
    return Results.Ok(c);

});

app.Run();

public interface IUsers
{
    string GetCurrentUserName();
}

class UsersManager : IUsers
{
    private readonly ITimeManager _timeManagar;

    public UsersManager(ITimeManager timeManagar)
    {
        _timeManagar = timeManagar;
        Thread.Sleep(4000);
    }
    public string GetCurrentUserName()
    {
        var time = _timeManagar.GetTime();
        return time + " SHIMON";
    }
}


public interface ITimeManager
{
    string GetTime();


}




public class ServerTimeManager : ITimeManager
{
    public string GetTime()
    {
        return DateTime.Now.ToShortTimeString();
    }
}


public  class ServerUTCManager : ITimeManager
{
    public ServerUTCManager()
    {
                
    }
    public string GetTime()
    {
        return DateTime.UtcNow.ToShortTimeString();
    }
}

