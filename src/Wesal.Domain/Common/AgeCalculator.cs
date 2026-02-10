namespace Wesal.Domain.Common;

public static class AgeCalculator
{
    public static int CalculateAge(DateOnly birthDate)
    {
        var age = EgyptTime.Now.Year - birthDate.Year;

        if (birthDate > DateOnly.FromDateTime(EgyptTime.Now).AddYears(-age))
            age--;

        return age;
    }
}