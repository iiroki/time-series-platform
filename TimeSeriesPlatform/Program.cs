using Iiroki.TimeSeriesPlatform.Extensions;
using Iiroki.TimeSeriesPlatform.Services;

// Configuration:
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiKeyAuthentication();

builder.Services.AddTspDatabase(builder.Configuration);
builder.Services.AddSingleton<IApiKeyService, ApiKeyService>();
builder.Services.AddSingleton<IMeasurementService, MeasurementService>();
builder.Services.AddScoped<IMetadataService, MetadataService>();
builder.Services.AddSingleton<IMeasurementListenerService, MeasurementListenerService>();
builder.Services.AddHostedService<NotificationService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddJsonContentTypeAttributes();

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
