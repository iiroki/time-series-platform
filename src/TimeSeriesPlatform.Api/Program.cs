using System.Text.Json;
using Iiroki.TimeSeriesPlatform.Application;
using JsonRpcX;
using JsonRpcX.Options;
using JsonRpcX.Transport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
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

builder.Services.AddSingleton(new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

builder
    .Services
    .AddJsonRpc()
    .AddJsonRpcControllersFromAssebly(new JsonRpcMethodOptions { NamingPolicy = JsonNamingPolicy.CamelCase });

// Request pipeline:
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

app.MapJsonRpcSchema("/json-rpc/schema");
app.MapJsonRpcHttp("/json-rpc");
app.MapJsonRpcWebSocket("/json-rpc/ws");

// Start:
await app.RunAsync();
