using Iiroki.TimeSeriesPlatform.Database;
using Iiroki.TimeSeriesPlatform.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Iiroki.TimeSeriesPlatform.Extensions;

public static class StartupExtensions
{
    /// <summary>
    /// Adds the basic API key authentication to services.
    /// </summary>
    public static void AddApiKeyAuthentication(this IServiceCollection services) =>
        services
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(Config.ApiKey, _ => { });

    /// <summary>
    /// Adds Time Series Platform database to services:<br/>
    /// - EF Core DB context<br/>
    /// - Npgsql data source.
    /// </summary>
    public static void AddTspDatabase(this IServiceCollection services, IConfiguration config)
    {
        var source = TspDbContext.CreateSource(config);
        services.AddSingleton(source);
        services.AddDbContext<TspDbContext>(opt => opt.UseNpgsql(source));
    }

    /// <summary>
    /// Adds Swagger documentation to services.
    /// </summary>
    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SupportNonNullableReferenceTypes();

            opt.AddSecurityDefinition(
                Config.ApiKey,
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Scheme = Config.ApiKey,
                    Name = ApiKeyAuthenticationHandler.ApiKeyHeader
                }
            );

            opt.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = Config.ApiKey }
                        },
                        new List<string>()
                    }
                }
            );
        });
    }

    /// <summary>
    /// Adds "Content-Type: application/json" attributes to services.
    /// </summary>
    public static void AddJsonContentTypeAttributes(this IServiceCollection services) =>
        services.AddMvcCore(opt =>
        {
            opt.Filters.Add(new ConsumesAttribute("application/json"));
            opt.Filters.Add(new ProducesAttribute("application/json"));
        });
}
