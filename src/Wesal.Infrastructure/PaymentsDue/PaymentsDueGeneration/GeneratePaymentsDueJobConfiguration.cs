using Microsoft.Extensions.Options;
using Quartz;

namespace Wesal.Infrastructure.PaymentsDue.PaymentsDueGeneration;

internal sealed class GeneratePaymentsDueJobConfiguration(IOptions<GeneratePaymentsDueOptions> options)
    : IConfigureOptions<QuartzOptions>
{
    private readonly GeneratePaymentsDueOptions _options = options.Value;

    public void Configure(QuartzOptions options)
    {
        if (!_options.Enabled)
            return;

        string jobName = typeof(GeneratePaymentsDueJob).FullName!;

        options
            .AddJob<GeneratePaymentsDueJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithInterval(_options.RunInterval).RepeatForever()));
    }
}