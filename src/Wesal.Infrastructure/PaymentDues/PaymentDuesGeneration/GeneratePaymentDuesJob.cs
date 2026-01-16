using Microsoft.Extensions.Options;
using Quartz;

namespace Wesal.Infrastructure.PaymentDues.PaymentDuesGeneration;

[DisallowConcurrentExecution]
internal sealed class GeneratePaymentDuesJob(
    IOptions<GeneratePaymentDuesOptions> options) : IJob
{
    private readonly GeneratePaymentDuesOptions _options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
    }
}