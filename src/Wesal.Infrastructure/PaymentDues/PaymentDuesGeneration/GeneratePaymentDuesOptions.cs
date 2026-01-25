namespace Wesal.Infrastructure.PaymentDues.PaymentDuesGeneration;

internal sealed class GeneratePaymentDuesOptions
{
    public const string SectionName = "BackgroundJobs:GeneratePaymentDues";

    public bool Enabled { get; init; }
    public float RunIntervalInMinutes { get; init; }
    public int BatchSize { get; init; }
    public int GenerateForNextDays { get; init; }

    public TimeSpan RunInterval => TimeSpan.FromMinutes(RunIntervalInMinutes);
}