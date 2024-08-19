using MoreMath.Api.Exceptions;
using MoreMath.Api.Extensions;
using MoreMath.Infrastructure;
using MoreMath.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoreMath.Application.UseCases.Authors.Commands;
using MoreMath.Api.Requests.Authors;
using MoreMath.Application.UseCases.Authors.Queries;

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

app.MapGet("/authors", async (IMediator mediator) =>
{
    var res = await mediator.Send(new GetAuthorsQuery());
    return res.ToHttpResult();
});

app.MapPost("/authors", async (IMediator mediator, [FromBody] CreateAuthorRequest request) =>
{
    var res = await mediator.Send(new CreateAuthorCommand(
        request.FirstName, request.LastName, request.AvatarUri, request.Info, request.ShortBio));
    return res.ToHttpResult();
});

app.MapPut("/authors", async (IMediator mediator, [FromBody] UpdateAuthorRequest request) =>
{
    var res = await mediator.Send(new UpdateAuthorCommand(
        request.Id, request.FirstName, request.LastName, request.AvatarUri, request.Info, request.ShortBio));
    return res.ToHttpResult();
});

app.Run();
