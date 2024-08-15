using MoreMath.Api.Exceptions;
using MoreMath.Api.Extensions;
using MoreMath.Infrastructure;
using MoreMath.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoreMath.Application.Requests;
using MoreMath.Application.UseCases.Authors.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddInfrastucture(builder.Configuration);
builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

app.ApplyMigrations();

app.UseSwaggerForDevelopment();
app.UseExceptionHandler(opt => { });
app.UseHttpsRedirection();

app.AddInfoEndpoint();

app.MapPost("/author", async (IMediator mediator, [FromBody] CreateAuthorRequest request) =>
{
    var res = await mediator.Send(new CreateAuthorCommand(request));
    return res.IsSuccessfull 
        ? Results.Created($"/author/{res.Value}", res.Value)
        : Results.BadRequest(res.Errors);
});

app.Run();
