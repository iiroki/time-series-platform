using Iiroki.TimeSeriesPlatform.Services;

// Configuration:
var builder = WebApplication.CreateBuilder(args);
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
