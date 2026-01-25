using System.Text.Json.Serialization;
using Serilog;
using Wesal.Api.Extensions;
using Wesal.Api.Middleware;
using Wesal.Infrastructure;
using Wesal.Presentation.Endpoints;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.ConfigureHttpJsonOptions(opts =>
        {
            opts.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            opts.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerDocumentation();

        builder.Services.AddWesal(builder.Configuration);

        builder.Services.AddCors(opts =>
            opts.AddDefaultPolicy(p =>
                p.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()));

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();

        app.UseSerilogRequestLogging();
        app.UseExceptionHandler();
        // app.UseAntiforgery();
        app.MapEndpoints();

        app.Run();
    }
}