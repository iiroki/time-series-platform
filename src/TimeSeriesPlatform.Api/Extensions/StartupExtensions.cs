using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Iiroki.TimeSeriesPlatform.Api.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;
using Iiroki.TimeSeriesPlatform.Core.Services;
using Iiroki.TimeSeriesPlatform.Api.Services;

namespace Iiroki.TimeSeriesPlatform.Api.Extensions;

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
            .AddSingleton<IApiKeyService, ApiKeyService>()
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(Config.ApiKey, _ => { });

    public static IServiceCollection AddMetadataService(this IServiceCollection services) =>
        services.AddScoped<IMetadataService, MetadataService>();

    public static IServiceCollection AddMeasurementService(this IServiceCollection services) =>
        services.AddScoped<IMeasurementService, MeasurementService>();

    /// <summary>
    /// Adds Swagger documentation to services.
    /// </summary>
    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Time Series Platform" });
            opt.OperationFilter<TspSwaggerFilter>();
            opt.SupportNonNullableReferenceTypes();
            opt.UseAllOfToExtendReferenceSchemas();
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
