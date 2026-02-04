using FluentValidation;
using MediatR;
using MediatR.Extensions.RegisterGenericHandlers;
using Microsoft.Extensions.DependencyInjection;
using Wesal.Application.Authentication.Credentials;
using Wesal.Application.Authentication.SignIn;
using Wesal.Application.Authentication.SignIn.Validators;
using Wesal.Application.Behaviours;
using Wesal.Contracts.Authentication;
using Wesal.Domain.Results;

namespace Wesal.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = AssemblyReference.Assembly;

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assembly);

            config.RegisterGenericHandlers = true;
            config.MaxTypesClosing = 0;

            config.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
            config.AddOpenBehavior(typeof(UnitOfWorkBehaviour<,>));
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.RegisterSignInHandlers();

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
        return services;
    }

    private static void RegisterSignInHandlers(this IServiceCollection services)
    {
        services.AddTransient(
            typeof(IRequestHandler<SignInCommand<EmailPasswordCredentials>, Result<JwtTokenResponse>>),
            typeof(SignInCommandHandler<EmailPasswordCredentials>));

        services.AddTransient(
            typeof(IRequestHandler<SignInCommand<NationalIdPasswordCredentials>, Result<JwtTokenResponse>>),
            typeof(SignInCommandHandler<NationalIdPasswordCredentials>));

        services.AddTransient(
            typeof(IRequestHandler<SignInCommand<UsernamePasswordCredentials>, Result<JwtTokenResponse>>),
            typeof(SignInCommandHandler<UsernamePasswordCredentials>));
    }
}