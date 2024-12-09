using Microsoft.EntityFrameworkCore;
using Patients.CatalogAPI.Apis;
using Patients.CatalogAPI.DataContext;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PatientsDataContext>
    (x=> x.UseSqlServer(builder.Configuration.GetConnectionString("myPatientsDb")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<PatientsDataContext>();
await dbContext.Database.EnsureCreatedAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UsePatientsApi();

app.Run();
