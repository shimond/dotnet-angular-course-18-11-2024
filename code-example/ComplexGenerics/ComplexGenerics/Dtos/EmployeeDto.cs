namespace ComplexGenerics.Dtos;

public sealed class EmployeeDto : PersonBaseDto
{
    public required string Department { get; init; }
    public required decimal Salary { get; init; }
    public required string EmployeeNumber { get; init; }
}
