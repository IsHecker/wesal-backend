namespace Wesal.Infrastructure.PaymentsDue.PaymentsDueGeneration;

internal sealed class GeneratePaymentsDueOptions
{
    public const string SectionName = "BackgroundJobs:GeneratePaymentsDue";

    public bool Enabled { get; init; }
    public float RunIntervalInMinutes { get; init; }
    public int BatchSize { get; init; }
    public int GenerateForNextDays { get; init; }

    public TimeSpan RunInterval => TimeSpan.FromMinutes(RunIntervalInMinutes);
}