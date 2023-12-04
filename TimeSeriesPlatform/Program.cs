using Iiroki.TimeSeriesPlatform;
using Iiroki.TimeSeriesPlatform.Middleware;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.AspNetCore.Authentication;

// Configuration:
var builder = WebApplication.CreateBuilder(args);
builder
    .Services
    .AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(Config.ApiKey, _ => { /* NOP */ });

builder.Services.AddSingleton<IApiKeyService, ApiKeyService>();


builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

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
