using Microsoft.Extensions.Options;
using Quartz;

namespace Wesal.Infrastructure.PaymentDues.PaymentDuesGeneration;

internal sealed class GeneratePaymentDuesJobConfiguration(IOptions<GeneratePaymentDuesOptions> options)
    : IConfigureOptions<QuartzOptions>
{
    private readonly GeneratePaymentDuesOptions _options = options.Value;

    public void Configure(QuartzOptions options)
    {
        if (!_options.Enabled)
            return;

        string jobName = typeof(GeneratePaymentDuesJob).FullName!;

        options
            .AddJob<GeneratePaymentDuesJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithInterval(_options.RunInterval).RepeatForever()));
    }
}