using ComplexGenerics.Dtos;
using ComplexGenerics.Enums;

namespace ComplexGenerics.Services.Handlers;

public abstract class PersonHandlerBase<TDto> : IPersonHandler<TDto> where TDto : PersonBaseDto
{
    private const int MinimumAge = 0;
    private const int MaximumAge = 150;

    public abstract PersonType HandlesType { get; }

    public async Task<string> HandleAsync(PersonBaseDto person)
    {
        var (isValid, errorMessage) = ValidateCommonProperties(person);
        if (!isValid)
        {
            throw new InvalidOperationException($"Validation failed: {errorMessage}");
        }

        return await HandleAsync((TDto)person);
    }

    public async Task<string> HandleAsync(TDto person)
    {
        var (isValid, errorMessage) = ValidateCommonProperties(person);
        if (!isValid)
        {
            throw new InvalidOperationException($"Validation failed: {errorMessage}");
        }

        return await HandleCoreAsync(person);
    }

    protected virtual (bool IsValid, string? ErrorMessage) ValidateCommonProperties(PersonBaseDto person)
    {
        if (string.IsNullOrWhiteSpace(person.Id))
        {
            return (false, "ID cannot be empty or whitespace.");
        }

        if (person.Age < MinimumAge || person.Age > MaximumAge)
        {
            return (false, $"Age must be between {MinimumAge} and {MaximumAge}.");
        }

        if (string.IsNullOrWhiteSpace(person.Name))
        {
            return (false, "Name cannot be empty or whitespace.");
        }

        return (true, null);
    }

    protected abstract Task<string> HandleCoreAsync(TDto person);
}

