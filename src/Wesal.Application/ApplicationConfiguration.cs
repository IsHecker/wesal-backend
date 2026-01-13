using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Wesal.Application;
using Wesal.Application.Behaviours;

namespace Enforcer.Common.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = AssemblyReference.Assembly;

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assembly);

            config.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
            config.AddOpenBehavior(typeof(UnitOfWorkBehaviour<,>));
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
        return services;
    }
}