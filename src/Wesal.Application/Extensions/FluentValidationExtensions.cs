using FluentValidation;

namespace Wesal.Application.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeEnumValue<T, TEnum>(
        this IRuleBuilder<T, string> ruleBuilder)
        where TEnum : struct, Enum
    {
        return ruleBuilder
            .Must(value => Enum.TryParse<TEnum>(value, true, out _))
            .WithMessage(AllowedValuesMessage<TEnum>());
    }

    private static string AllowedValuesMessage<TEnum>() where TEnum : struct, Enum
    {
        return $"'{nameof(TEnum)}' must be one of: {string.Join(", ", Enum.GetNames(typeof(TEnum)))}";
    }
}