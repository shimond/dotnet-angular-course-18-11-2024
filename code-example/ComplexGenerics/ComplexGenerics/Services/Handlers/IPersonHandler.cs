using ComplexGenerics.Dtos;
using ComplexGenerics.Enums;

namespace ComplexGenerics.Services.Handlers;

public interface IPersonHandler
{
    PersonType HandlesType { get; }
    Task<string> HandleAsync(PersonBaseDto person);
}

public interface IPersonHandler<TDto> : IPersonHandler where TDto : PersonBaseDto
{
    Task<string> HandleAsync(TDto person);
}

