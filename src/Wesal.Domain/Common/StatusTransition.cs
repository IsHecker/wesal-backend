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
        return Validate(current, [requiredStatus], targetStatus);
    }

    public static Result Validate<TEnum>(
        TEnum current,
        TEnum[] requiredStatuses,
        TEnum targetStatus)
        where TEnum : struct, Enum
    {
        if (EqualityComparer<TEnum>.Default.Equals(current, targetStatus))
            return Error.Validation(
                $"General.IsAlready{current}",
                $"This Entity is already '{current}'");

        if (!requiredStatuses.Contains(current))
            return Error.Validation(
                "General.InvalidStatusTransition",
                $"Cannot change status from '{current}' to '{targetStatus}'.");

        return Result.Success;
    }
}