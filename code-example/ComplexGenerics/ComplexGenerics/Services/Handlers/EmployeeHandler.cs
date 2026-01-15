using ComplexGenerics.Dtos;
using ComplexGenerics.Enums;

namespace ComplexGenerics.Services.Handlers;

public sealed class EmployeeHandler : PersonHandlerBase<EmployeeDto>
{
    public override PersonType HandlesType => PersonType.Employee;

    protected override Task<string> HandleCoreAsync(EmployeeDto employee)
    {
        var result = $"Employee processed successfully: {employee.Name} " +
                    $"(ID: {employee.Id}, Employee#: {employee.EmployeeNumber}) " +
                    $"works in {employee.Department} department " +
                    $"with salary ${employee.Salary:N2}";

        return Task.FromResult(result);
    }
}

