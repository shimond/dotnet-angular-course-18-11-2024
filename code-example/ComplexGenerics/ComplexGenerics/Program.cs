using System.Text.Json;
using ComplexGenerics.Dtos;
using ComplexGenerics.Enums;
using ComplexGenerics.Services.Handlers;
using ComplexGenerics.Services.Resolver;

var builder = WebApplication.CreateBuilder(args);

//singleton or scope registrations for handlers
builder.Services.AddSingleton<IPersonHandler, EmployeeHandler>();
builder.Services.AddSingleton<IPersonHandler, ManagerHandler>();
builder.Services.AddSingleton<IPersonHandler, StudentHandler>();

builder.Services.AddSingleton<IPersonHandlerResolver, PersonHandlerResolver>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/person/handle", async (
    PersonRequest request,
    IPersonHandlerResolver handlerResolver) =>
{
    try
    {
        PersonBaseDto person = request.PersonType switch
        {
            PersonType.Employee => request.Payload.Deserialize<EmployeeDto>()
                ?? throw new InvalidOperationException("Failed to deserialize Employee payload."),
            PersonType.Manager => request.Payload.Deserialize<ManagerDto>()
                ?? throw new InvalidOperationException("Failed to deserialize Manager payload."),
            PersonType.Student => request.Payload.Deserialize<StudentDto>()
                ?? throw new InvalidOperationException("Failed to deserialize Student payload."),
            _ => throw new ArgumentException($"Unsupported PersonType: {request.PersonType}")
        };

        var handler = handlerResolver.Resolve(request.PersonType);
        if (handler is null)
        {
            return Results.Problem(
                detail: $"No handler registered for PersonType: {request.PersonType}",
                statusCode: StatusCodes.Status500InternalServerError);
        }

        var result = await handler.HandleAsync(person);

        return Results.Ok(new { message = result });
    }
    catch (InvalidOperationException ex) when (ex.Message.StartsWith("Validation failed"))
    {
        return Results.BadRequest(new { error = ex.Message.Replace("Validation failed: ", "") });
    }
    catch (JsonException ex)
    {
        return Results.BadRequest(new { error = "Invalid JSON payload", details = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: ex.Message,
            statusCode: StatusCodes.Status500InternalServerError);
    }
})
.WithName("HandlePerson")
.WithDescription("Processes person data using enum-based polymorphic handler resolution")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError);

app.Run();
