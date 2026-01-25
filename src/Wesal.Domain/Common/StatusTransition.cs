using Wesal.Domain.Results;

namespace Wesal.Domain.Common;

internal sealed class StatusTransition
{
    public static Result Validate<TEnum>(
        TEnum current,
        TEnum requiredStatus,
        TEnum targetStatus)
        where TEnum : struct, Enum
    {
        if (EqualityComparer<TEnum>.Default.Equals(current, targetStatus))
            Error.Validation(
                $"General.IsAlready{current}",
                $"This Entity is already '{current}'");

        if (!EqualityComparer<TEnum>.Default.Equals(current, requiredStatus))
            Error.Validation(
                "General.InvalidStatusTransition",
                $"Cannot change status from '{current}' to '{targetStatus}'.");

        return Result.Success;
    }
}