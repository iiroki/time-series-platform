using Iiroki.TimeSeriesPlatform.Database;
using Iiroki.TimeSeriesPlatform.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Iiroki.TimeSeriesPlatform.Extensions;

public static class StartupExtensions
{
    public static void AddTspDatabase(this IServiceCollection services, IConfiguration config)
    {
        var source = TspDbContext.CreateSource(config);
        services.AddSingleton(source);
        services.AddDbContext<TspDbContext>(opt => opt.UseNpgsql(source));
    }

    public static void AddSwaggerDoc(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            var id = "API key";

            opt.AddSecurityDefinition(
                id,
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
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = id }
                        },
                        new List<string>()
                    }
                }
            );
        });
    }
}
