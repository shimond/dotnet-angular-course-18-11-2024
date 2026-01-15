using ComplexGenerics.Dtos;

namespace ComplexGenerics.Services.Common;

public sealed class PersonCommonService : IPersonCommonService
{
    private const int MinimumAge = 0;
    private const int MaximumAge = 150;

    public Task<(bool IsValid, string? ErrorMessage)> ValidateCommonPropertiesAsync(PersonBaseDto person)
    {
        if (string.IsNullOrWhiteSpace(person.Id))
        {
            return Task.FromResult((false, "ID cannot be empty or whitespace."));
        }
        if (person.Age < MinimumAge || person.Age > MaximumAge)
        {
            return Task.FromResult((false, $"Age must be between {MinimumAge} and {MaximumAge}."));
        }
        if (string.IsNullOrWhiteSpace(person.Name))
        {
            return Task.FromResult((false, "Name cannot be empty or whitespace."));
        }

        return Task.FromResult<(bool IsValid, string? ErrorMessage)>((true, null));
    }
}
