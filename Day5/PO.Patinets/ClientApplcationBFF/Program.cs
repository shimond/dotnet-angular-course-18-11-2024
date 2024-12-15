var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(c => c.AddDefaultPolicy(o =>

o.AllowAnyHeader()
.AllowCredentials()
.SetIsOriginAllowed(o => true)
.AllowAnyMethod()));
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();


var app = builder.Build();

app.MapDefaultEndpoints();
app.UseCors();
app.MapReverseProxy();

app.Run();
