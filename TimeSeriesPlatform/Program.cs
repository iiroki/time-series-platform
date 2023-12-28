using Iiroki.TimeSeriesPlatform;
using Iiroki.TimeSeriesPlatform.Extensions;
using Iiroki.TimeSeriesPlatform.Middleware;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

// Configuration:
var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(Config.ApiKey, _ => { });

builder.Services.AddTspDatabase(builder.Configuration);
builder.Services.AddSingleton<IApiKeyService, ApiKeyService>();
builder.Services.AddSingleton<IMeasurementService, MeasurementService>();
builder.Services.AddScoped<IMetadataService, MetadataService>();
builder.Services.AddSingleton<IMeasurementListenerService, MeasurementListenerService>();
builder.Services.AddHostedService<NotificationService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerDoc();
builder
    .Services
    .AddMvcCore(opt =>
    {
        opt.Filters.Add(new ConsumesAttribute("application/json"));
        opt.Filters.Add(new ProducesAttribute("application/json"));
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
