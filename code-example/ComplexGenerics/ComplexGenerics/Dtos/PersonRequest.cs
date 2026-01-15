using System.Text.Json;
using ComplexGenerics.Enums;

namespace ComplexGenerics.Dtos;

public sealed class PersonRequest
{
    public required PersonType PersonType { get; init; }
    public required JsonElement Payload { get; init; }
}
