using ComplexGenerics.Dtos;
using ComplexGenerics.Enums;

namespace ComplexGenerics.Services.Handlers;

public sealed class StudentHandler : PersonHandlerBase<StudentDto>
{
    public override PersonType HandlesType => PersonType.Student;

    protected override Task<string> HandleCoreAsync(StudentDto student)
    {
        var result = $"Student processed successfully: {student.Name} " +
                    $"(ID: {student.Id}, Student ID: {student.StudentId}) " +
                    $"is a Year {student.Year} {student.Major} major " +
                    $"with GPA: {student.Gpa:F2}";

        return Task.FromResult(result);
    }
}

