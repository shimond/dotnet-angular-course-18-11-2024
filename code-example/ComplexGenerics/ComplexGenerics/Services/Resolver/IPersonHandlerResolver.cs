using ComplexGenerics.Enums;
using ComplexGenerics.Services.Handlers;

namespace ComplexGenerics.Services.Resolver;

public interface IPersonHandlerResolver
{
    IPersonHandler? Resolve(PersonType personType);
}
