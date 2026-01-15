using ComplexGenerics.Enums;
using ComplexGenerics.Services.Handlers;

namespace ComplexGenerics.Services.Resolver;

public sealed class PersonHandlerResolver : IPersonHandlerResolver
{
    private readonly Dictionary<PersonType, IPersonHandler> _handlers;

    public PersonHandlerResolver(IEnumerable<IPersonHandler> handlers)
    {
        _handlers = handlers.ToDictionary(h => h.HandlesType, h => h);
    }

    public IPersonHandler? Resolve(PersonType personType)
    {
        return _handlers.GetValueOrDefault(personType);
    }
}
