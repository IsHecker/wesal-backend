using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Quartz;
using Wesal.Application;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Caching;
using Wesal.Application.Complaints;
using Wesal.Application.Data;
using Wesal.Application.ObligationAlerts;
using Wesal.Application.Visitations;
using Wesal.Infrastructure.Alimonies;
using Wesal.Infrastructure.Caching;
using Wesal.Infrastructure.Children;
using Wesal.Infrastructure.Complaints;
using Wesal.Infrastructure.CourtCases;
using Wesal.Infrastructure.CourtStaffs;
using Wesal.Infrastructure.Custodies;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;
using Wesal.Infrastructure.Families;
using Wesal.Infrastructure.Notifications;
using Wesal.Infrastructure.ObligationAlerts;
using Wesal.Infrastructure.ObligationAlerts.MissedVisitationsDetection;
using Wesal.Infrastructure.ObligationAlerts.OverdueAlimonyPaymentsDetection;
using Wesal.Infrastructure.Parents;
using Wesal.Infrastructure.PaymentDues;
using Wesal.Infrastructure.PaymentDues.PaymentDuesGeneration;
using Wesal.Infrastructure.Payments;
using Wesal.Infrastructure.Schools;
using Wesal.Infrastructure.VisitationLocations;
using Wesal.Infrastructure.Visitations.VisitationSessionsGeneration;
using Wesal.Infrastructure.VisitCenterStaffs;
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

        services.AddOptions(configuration);
        services.AddRepositories();
        services.AddServices();
        services.AddBackgroundJobs(configuration);
    }

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<VisitationOptions>(
            configuration.GetSection(VisitationOptions.SectionName));

        services.Configure<ObligationAlertOptions>(
            configuration.GetSection(ObligationAlertOptions.SectionName));

        services.Configure<ComplaintOptions>(
            configuration.GetSection(ComplaintOptions.SectionName));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IParentRepository, ParentRepository>();
        services.AddScoped<ISchoolRepository, SchoolRepository>();
        services.AddScoped<IFamilyRepository, FamilyRepository>();
        services.AddScoped<IChildRepository, ChildRepository>();
        services.AddScoped<ICourtCaseRepository, CourtCaseRepository>();
        services.AddScoped<ICustodyRepository, CustodyRepository>();
        services.AddScoped<IVisitationLocationRepository, VisitationLocationRepository>();
        services.AddScoped<IAlimonyRepository, AlimonyRepository>();
        services.AddScoped<IPaymentDueRepository, PaymentDueRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ICourtStaffRepository, CourtStaffRepository>();
        services.AddScoped<IObligationAlertRepository, ObligationAlertRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IVisitCenterStaffRepository, VisitCenterStaffRepository>();
        services.AddScoped<IComplaintRepository, ComplaintRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ObligationAlertService>();
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