using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
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
using Wesal.Infrastructure.Interceptors;
using Wesal.Infrastructure.Notifications;
using Wesal.Infrastructure.Notifications.Services;
using Wesal.Infrastructure.ObligationAlerts;
using Wesal.Infrastructure.ObligationAlerts.MissedVisitationsDetection;
using Wesal.Infrastructure.ObligationAlerts.OverdueAlimonyPaymentsDetection;
using Wesal.Infrastructure.Parents;
using Wesal.Infrastructure.PaymentsDue;
using Wesal.Infrastructure.PaymentsDue.PaymentDueReminder;
using Wesal.Infrastructure.PaymentsDue.PaymentsDueGeneration;
using Wesal.Infrastructure.Payments;
using Wesal.Infrastructure.Schools;
using Wesal.Infrastructure.UserDevices;
using Wesal.Infrastructure.VisitationLocations;
using Wesal.Infrastructure.Visitations.VisitationReminder;
using Wesal.Infrastructure.Visitations.VisitationSessionsGeneration;
using Wesal.Infrastructure.VisitCenterStaffs;
using Wesal.Presentation.Endpoints;
using Microsoft.AspNetCore.Identity;
using Wesal.Domain.Entities.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Wesal.Infrastructure.Options;
using Wesal.Infrastructure.Authentication;
using Wesal.Infrastructure.Users;
using Wesal.Infrastructure.FamilyCourts;
using Wesal.Application.Authentication;
using Wesal.Application.Authentication.Credentials;
using Wesal.Infrastructure.Authentication.Strategies;
using Wesal.Application.Abstractions.Authentication;
using Wesal.Infrastructure.Authentication.Services;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        services.AddAuthenticationInternal(configuration);
        services.AddNotification(configuration);
        services.AddDocumentServices(configuration);
        services.AddRepositories();
        services.AddServices();
        services.AddBackgroundJobs(configuration);
    }

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton<PublishDomainEventsInterceptor>();
        services.AddDbContext<WesalDbContext>((sp, opts) =>
            opts.UseSqlServer(configuration.GetConnectionString("Database"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>())
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
        services.AddScoped<IUserDeviceRepository, UserDeviceRepository>();
        services.AddScoped<IFamilyCourtRepository, FamilyCourtRepository>();
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


        services.Configure<GeneratePaymentsDueOptions>(
            configuration.GetSection(GeneratePaymentsDueOptions.SectionName));

        services.ConfigureOptions<GeneratePaymentsDueJobConfiguration>();


        services.Configure<DetectMissedVisitationsOptions>(
            configuration.GetSection(DetectMissedVisitationsOptions.SectionName));

        services.ConfigureOptions<DetectMissedVisitationsJobConfiguration>();


        services.Configure<DetectOverdueAlimonyPaymentsOptions>(
            configuration.GetSection(DetectOverdueAlimonyPaymentsOptions.SectionName));

        services.ConfigureOptions<DetectOverdueAlimonyPaymentsJobConfiguration>();


        services.Configure<VisitationReminderOptions>(
            configuration.GetSection(VisitationReminderOptions.SectionName));

        services.ConfigureOptions<VisitationReminderJobConfiguration>();


        services.Configure<PaymentDueReminderOptions>(
            configuration.GetSection(PaymentDueReminderOptions.SectionName));

        services.ConfigureOptions<PaymentDueReminderJobConfiguration>();
    }

    private static void AddDocumentServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinaryOptions>(
            configuration.GetSection(CloudinaryOptions.SectionName));

        services.AddScoped<ICloudinaryService, CloudinaryService>();
    }

    public static IServiceCollection AddNotification(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var serviceAccount = configuration.GetSection("FirebaseCloudMessaging")
            .Get<JsonCredentialParameters>();

        if (serviceAccount is null)
            throw new InvalidOperationException("Firebase service account key path is not configured.");

        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJsonParameters(serviceAccount)
            });
        }

        services.AddSingleton(FirebaseMessaging.DefaultInstance);
        services.AddScoped<FcmService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }

    internal static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()!;
        services.AddSingleton(jwtOptions);

        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer((options) =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = CustomClaims.Role,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = false,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        return context.Response.WriteAsJsonAsync(new
                        {
                            message = "Authentication token is missing or invalid"
                        });
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        return context.Response.WriteAsJsonAsync(new
                        {
                            message = "You do not have permission to access this resource."
                        });
                    }
                };
            });

        services.AddAuthorization();

        services.AddAuthorizationBuilder()
            .AddPolicy(CustomPolicies.SystemAdminOnly, policy => policy.RequireRole(UserRole.SystemAdmin))
            .AddPolicy(CustomPolicies.FamilyCourtAdminOnly, policy => policy.RequireRole(UserRole.FamilyCourt))
            .AddPolicy(CustomPolicies.CourtStaffOnly, policy => policy.RequireRole(UserRole.CourtStaff))
            .AddPolicy(CustomPolicies.CourtManagement, policy => policy.RequireRole(UserRole.FamilyCourt, UserRole.CourtStaff))
            .AddPolicy(CustomPolicies.ParentsOnly, policy => policy.RequireRole(UserRole.Parent))
            .AddPolicy(CustomPolicies.SchoolsOnly, policy => policy.RequireRole(UserRole.School))
            .AddPolicy(CustomPolicies.VisitCenterStaffOnly, policy => policy.RequireRole(UserRole.VisitCenterStaff));

        services.AddHttpContextAccessor();

        services.AddSingleton<TokenGeneratorService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<AuthenticationService>();

        services.AddScoped<IAuthenticationStrategy<EmailPasswordCredentials>, EmailPasswordAuthenticationStrategy>();
        services.AddScoped<IAuthenticationStrategy<NationalIdPasswordCredentials>, NationalIdPasswordAuthenticationStrategy>();
        services.AddScoped<IAuthenticationStrategy<UsernamePasswordCredentials>, UsernamePasswordAuthenticationStrategy>();

        return services;
    }
}