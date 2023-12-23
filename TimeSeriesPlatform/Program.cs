using Iiroki.TimeSeriesPlatform;
using Iiroki.TimeSeriesPlatform.Extensions;
using Iiroki.TimeSeriesPlatform.Middleware;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

// Configuration:
var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(Config.ApiKey, _ => { });

builder.Services.AddTspDbContext();
builder.Services.AddSingleton<IApiKeyService, ApiKeyService>();
builder.Services.AddScoped<IMetadataService, MetadataService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(opt =>
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
            // Description = $"Header: {ApiKeyAuthenticationHandler.ApiKeyHeader}"
        }
    );

    opt.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = id }
                },
                new List<string>()
            }
        }
    );
});

// Request pipeline:
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Start:
await app.RunAsync();
