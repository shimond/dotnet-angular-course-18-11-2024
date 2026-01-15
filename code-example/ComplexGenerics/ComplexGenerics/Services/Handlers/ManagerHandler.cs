using ComplexGenerics.Dtos;
using ComplexGenerics.Enums;

namespace ComplexGenerics.Services.Handlers;

public sealed class ManagerHandler : PersonHandlerBase<ManagerDto>
{
    public override PersonType HandlesType => PersonType.Manager;

    protected override Task<string> HandleCoreAsync(ManagerDto manager)
    {
        var result = $"Manager processed successfully: {manager.Name} " +
                    $"(ID: {manager.Id}) manages {manager.TeamSize} people " +
                    $"in {manager.Department} department " +
                    $"with salary ${manager.Salary:N2}. " +
                    $"Direct reports: {string.Join(", ", manager.DirectReports)}";

        return Task.FromResult(result);
    }
}

