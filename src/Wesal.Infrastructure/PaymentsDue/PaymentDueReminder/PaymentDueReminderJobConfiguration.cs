using Microsoft.Extensions.Options;
using Quartz;

namespace Wesal.Infrastructure.PaymentsDue.PaymentDueReminder;

internal sealed class PaymentDueReminderJobConfiguration(IOptions<PaymentDueReminderOptions> options)
    : IConfigureOptions<QuartzOptions>
{
    private readonly PaymentDueReminderOptions _options = options.Value;

    public void Configure(QuartzOptions options)
    {
        if (!_options.Enabled)
            return;

        string jobName = typeof(PaymentDueReminderJob).FullName!;

        options
            .AddJob<PaymentDueReminderJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithInterval(_options.RunInterval).RepeatForever()));
    }
}