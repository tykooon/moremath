using MoreMath.Api.Exceptions;
using MoreMath.Api.Extensions;
using MoreMath.Infrastructure;
using MoreMath.Application;
using MoreMath.Api.Authentication.ApiKey;

var builder = WebApplication.CreateBuilder(args);
builder.AddSerilogLogging();
builder.Configuration.AddSecrets();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();
builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddInfrastucture(builder.Configuration);
builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
builder.Services.AddScoped<ApiKeyAuthenticationFilter>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.ApplyMigrations();

app.UseSwaggerForDevelopment();
app.UseExceptionHandler(opt => { });

app.MapControllers();
app.AddInfoEndpoint();

app.Run();
