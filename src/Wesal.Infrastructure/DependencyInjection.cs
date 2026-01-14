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
using Wesal.Infrastructure.Parents;
using Wesal.Infrastructure.PaymentGateway;
using Wesal.Infrastructure.Payments;
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
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IParentRepository, ParentRepository>();
        services.AddScoped<IFamilyRepository, FamilyRepository>();
        services.AddScoped<IChildRepository, ChildRepository>();
        services.AddScoped<ICourtCaseRepository, CourtCaseRepository>();
        services.AddScoped<ICustodyRepository, CustodyRepository>();
        services.AddScoped<IAlimonyRepository, AlimonyRepository>();
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
}