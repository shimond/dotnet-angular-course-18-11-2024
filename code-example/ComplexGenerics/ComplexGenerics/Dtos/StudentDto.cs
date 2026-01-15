namespace ComplexGenerics.Dtos;

public sealed class StudentDto : PersonBaseDto
{
    public required string Major { get; init; }
    public required double Gpa { get; init; }
    public required int Year { get; init; }
    public required string StudentId { get; init; }
}
