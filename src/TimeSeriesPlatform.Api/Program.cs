using Iiroki.TimeSeriesPlatform.Api.Extensions;
using Iiroki.TimeSeriesPlatform.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// Configuration:
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJsonOptions();
builder.Services.AddApiKeyAuthentication();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddJsonContentTypeAttributes();

// Request pipeline:
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

// Start:
await app.RunAsync();
