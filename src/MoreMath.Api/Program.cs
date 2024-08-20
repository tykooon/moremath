using MoreMath.Api.Exceptions;
using MoreMath.Api.Extensions;
using MoreMath.Infrastructure;
using MoreMath.Application;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();
builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddInfrastucture(builder.Configuration);
builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

app.ApplyMigrations();

app.UseSwaggerForDevelopment();
app.UseExceptionHandler(opt => { });
app.UseHttpsRedirection();

app.MapControllers();
app.AddInfoEndpoint();

app.Run();
