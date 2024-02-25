using Iiroki.TimeSeriesPlatform.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// Configuration:
var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApiKeyAuthentication()
    .AddJsonOptions()
    .AddJsonContentTypeAttributes()
    .AddSwaggerDocumentation()
    .AddControllers();

// Request pipeline:
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

// Start:
await app.RunAsync();
