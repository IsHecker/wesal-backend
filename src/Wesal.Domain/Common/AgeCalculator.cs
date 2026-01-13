namespace Wesal.Domain.Common;

public static class AgeCalculator
{
    public static int CalculateAge(DateTime birthDate)
    {
        var age = DateTime.UtcNow.Year - birthDate.Year;

        if (birthDate.Date > DateTime.UtcNow.AddYears(-age))
            age--;

        return age;
    }
}