using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Quartz;
using Wesal.Application;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Caching;
using Wesal.Application.Complaints;
using Wesal.Application.Data;
using Wesal.Application.ObligationAlerts;
using Wesal.Application.Visitations;
using Wesal.Infrastructure.Alimonies;
using Wesal.Infrastructure.Caching;
using Wesal.Infrastructure.Children;
using Wesal.Infrastructure.CloudinaryStorage;
using Wesal.Infrastructure.Complaints;
using Wesal.Infrastructure.ComplianceMetrics;
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
        services.AddOptions(configuration);
        services.AddCache();
        services.AddDbContext(configuration);
        services.AddDocumentServices();
        services.AddRepositories();
        services.AddServices();
        services.AddBackgroundJobs(configuration);
    }

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WesalDbContext>((sp, opts) =>
            opts.UseSqlServer(configuration.GetConnectionString("Database"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .LogTo(_ => { }, LogLevel.None));


        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<WesalDbContext>());
        services.AddScoped<IWesalDbContext>(sp => sp.GetRequiredService<WesalDbContext>());
    }

    private static void AddCache(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.TryAddSingleton<ICacheService, CacheService>();
    }

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<VisitationOptions>(
            configuration.GetSection(VisitationOptions.SectionName));

        services.Configure<ObligationAlertOptions>(
            configuration.GetSection(ObligationAlertOptions.SectionName));

        services.Configure<ComplaintOptions>(
            configuration.GetSection(ComplaintOptions.SectionName));

        services.Configure<CloudinaryOptions>(
            configuration.GetSection(CloudinaryOptions.SectionName));
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
        services.AddScoped<IComplianceMetricsRepository, ComplianceMetricsRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ObligationAlertService>();
    }

    private static void AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

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

    private static void AddDocumentServices(this IServiceCollection services)
    {
        services.AddScoped<ICloudinaryService, CloudinaryService>();
    }
}