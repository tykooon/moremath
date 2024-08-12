using MoreMath.Api.Exceptions;
using MoreMath.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseSwaggerForDevelopment();
app.UseExceptionHandler(opt => { });
app.UseHttpsRedirection();

app.AddInfoEndpoint();

app.Run();
