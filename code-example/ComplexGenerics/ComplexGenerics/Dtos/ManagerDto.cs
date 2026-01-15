namespace ComplexGenerics.Dtos;

public sealed class ManagerDto : PersonBaseDto
{
    public required string Department { get; init; }
    public required decimal Salary { get; init; }
    public required int TeamSize { get; init; }
    public required string[] DirectReports { get; init; }
}
