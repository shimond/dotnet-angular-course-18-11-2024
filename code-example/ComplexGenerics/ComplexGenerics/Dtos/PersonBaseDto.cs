namespace ComplexGenerics.Dtos;

public abstract class PersonBaseDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required int Age { get; init; }
}
