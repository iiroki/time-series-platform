using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Iiroki.TimeSeriesPlatform.Database;
using Iiroki.TimeSeriesPlatform.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Iiroki.TimeSeriesPlatform.Extensions;

public static class StartupExtensions
{
    /// <summary>
    /// Adds default JSON options to services.
    /// </summary>
    public static void AddJsonOptions(this IServiceCollection services) =>
        services.Configure<JsonOptions>(opt =>
        {
            opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opt.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

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
            opt.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Time Series Platform",
                    Description = "Time Series Platform (TSP) is a simple web app to work with time series data."
                }
            );

            opt.OperationFilter<TspSwaggerFilter>();
            opt.SupportNonNullableReferenceTypes();
            opt.IncludeXmlComments(
                Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml")
            );

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

    private class TspSwaggerFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext ctx)
        {
            // Read from the endpoint
            var roles = ctx.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(a => a.Roles)
                .ToList();

            // Read from the controller if not defined on endpoint level
            if (roles.Count == 0 && ctx.MethodInfo.DeclaringType != null)
            {
                roles = ctx.MethodInfo
                    .DeclaringType
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Select(a => a.Roles)
                    .ToList();
            }

            if (roles.Where(r => !string.IsNullOrWhiteSpace(r)).ToList().Count > 0)
            {
                operation.Description += $"<p><b>Roles:</b> {string.Join(", ", roles)}</p>";
            }
        }
    }
}
