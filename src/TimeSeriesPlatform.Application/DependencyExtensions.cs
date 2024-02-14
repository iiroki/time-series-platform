using Iiroki.TimeSeriesPlatform.Domain.Services;
using Iiroki.TimeSeriesPlatform.Infrastructure;
using Iiroki.TimeSeriesPlatform.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iiroki.TimeSeriesPlatform.Application;

public static class DependencyExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) =>
        services.AddTspDatabase(config).AddSqids().AddMetadataService().AddMeasurementService();

    public static IServiceCollection AddMetadataService(this IServiceCollection services) =>
        services.AddScoped<IMetadataService, MetadataService>();

    public static IServiceCollection AddMeasurementService(this IServiceCollection services) =>
        services.AddScoped<IMeasurementService, MeasurementService>();
}
