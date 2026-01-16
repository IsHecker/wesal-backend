using Enforcer.Common.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Quartz;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Caching;
using Wesal.Application.Data;
using Wesal.Infrastructure.Alimonies;
using Wesal.Infrastructure.Caching;
using Wesal.Infrastructure.Children;
using Wesal.Infrastructure.CourtCases;
using Wesal.Infrastructure.CourtStaffs;
using Wesal.Infrastructure.Custodies;
using Wesal.Infrastructure.Database;
using Wesal.Infrastructure.Families;
using Wesal.Infrastructure.Notifications;
using Wesal.Infrastructure.ObligationAlerts;
using Wesal.Infrastructure.ObligationAlerts.MissedVisitationsDetection;
using Wesal.Infrastructure.ObligationAlerts.OverdueAlimonyPaymentsDetection;
using Wesal.Infrastructure.Parents;
using Wesal.Infrastructure.PaymentDues;
using Wesal.Infrastructure.PaymentDues.PaymentDuesGeneration;
using Wesal.Infrastructure.PaymentGateway;
using Wesal.Infrastructure.Payments;
using Wesal.Infrastructure.Visitations.VisitationSessionsGeneration;
using Wesal.Presentation.Endpoints;

namespace Wesal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWesal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        services.AddApplication();

        services.AddInfrastructure(configuration);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDistributedMemoryCache();
        services.TryAddSingleton<ICacheService, CacheService>();

        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);


        services.AddDbContext<WesalDbContext>((sp, opts) =>
            opts.UseSqlServer(configuration.GetConnectionString("Database"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .LogTo(_ => { }, LogLevel.None));


        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<WesalDbContext>());
        services.AddScoped<IWesalDbContext>(sp => sp.GetRequiredService<WesalDbContext>());

        services.AddRepositories();
        services.AddServices();
        services.AddBackgroundJobs(configuration);
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IParentRepository, ParentRepository>();
        services.AddScoped<IFamilyRepository, FamilyRepository>();
        services.AddScoped<IChildRepository, ChildRepository>();
        services.AddScoped<ICourtCaseRepository, CourtCaseRepository>();
        services.AddScoped<ICustodyRepository, CustodyRepository>();
        services.AddScoped<IAlimonyRepository, AlimonyRepository>();
        services.AddScoped<IPaymentDueRepository, PaymentDueRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ICourtStaffRepository, CourtStaffRepository>();
        services.AddScoped<IObligationAlertRepository, ObligationAlertRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
        services.AddScoped<INotificationService, NotificationService>();
    }

    private static void AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GenerateVisitationSessionsOptions>(
            configuration.GetSection(GenerateVisitationSessionsOptions.SectionName));

        services.ConfigureOptions<GenerateVisitationSessionsJobConfiguration>();


        services.Configure<GeneratePaymentDuesOptions>(
            configuration.GetSection(GeneratePaymentDuesOptions.SectionName));

        services.ConfigureOptions<GeneratePaymentDuesJobConfiguration>();


        services.Configure<DetectMissedVisitationsOptions>(
            configuration.GetSection(DetectMissedVisitationsOptions.SectionName));

        services.ConfigureOptions<DetectMissedVisitationsJobConfiguration>();


        services.Configure<DetectOverdueAlimonyPaymentsOptions>(
            configuration.GetSection(DetectOverdueAlimonyPaymentsOptions.SectionName));

        services.ConfigureOptions<DetectOverdueAlimonyPaymentsJobConfiguration>();
    }
}