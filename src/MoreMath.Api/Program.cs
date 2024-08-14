using MoreMath.Api.Exceptions;
using MoreMath.Api.Extensions;
using MoreMath.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddInfrastucture(builder.Configuration);

var app = builder.Build();

app.ApplyMigrations();

app.UseSwaggerForDevelopment();
app.UseExceptionHandler(opt => { });
app.UseHttpsRedirection();

app.AddInfoEndpoint();

app.Run();
