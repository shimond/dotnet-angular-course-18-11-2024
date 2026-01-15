using ComplexGenerics.Dtos;

namespace ComplexGenerics.Services.Common;

public interface IPersonCommonService
{
    Task<(bool IsValid, string? ErrorMessage)> ValidateCommonPropertiesAsync(PersonBaseDto person);
}
