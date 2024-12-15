var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(c=> c.AddDefaultPolicy(o=> o.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


var app = builder.Build();
app.UseCors();
app.MapReverseProxy();

app.Run();
