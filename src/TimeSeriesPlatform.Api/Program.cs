using Iiroki.TimeSeriesPlatform.Api.Extensions;
using Iiroki.TimeSeriesPlatform.Core.Extensions;
using Iiroki.TimeSeriesPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// Configuration:
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqids();
builder.Services.AddJsonOptions();
builder.Services.AddApiKeyAuthentication();
builder.Services.AddTspDatabase(builder.Configuration);

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
