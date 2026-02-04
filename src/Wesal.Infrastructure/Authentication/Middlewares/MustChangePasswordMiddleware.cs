using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Wesal.Application.Authentication;
using Wesal.Domain.Results;
using Wesal.Presentation.EndpointResults;

namespace Wesal.Infrastructure.Authentication.Middlewares;

public sealed class MustChangePasswordMiddleware(RequestDelegate next)
{
    public Task InvokeAsync(HttpContext context)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
            return next(context);

        if (!bool.TryParse(user.FindFirstValue(CustomClaims.PasswordChangeRequired), out var passwordChangeRequired))
            return next(context);

        var path = context.Request.Path.Value?.ToLower();

        var isChangePasswordEndpoint = path == "/api/users/change-password";

        if (passwordChangeRequired && !isChangePasswordEndpoint)
        {
            return ErrorResponse(context, Error.Forbidden(
                "Users.PasswordChangeRequired",
                "You must change your temporary password before using the app."));
        }

        return next(context);
    }

    private static async Task ErrorResponse(HttpContext context, Error error)
    {
        var problemResult = ApiResults.Problem(error);
        await problemResult.ExecuteAsync(context);
    }
}