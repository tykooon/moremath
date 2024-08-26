using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoreMath.Api.Extensions;
using MoreMath.Api.Requests.Authors;
using MoreMath.Dto.Dtos;
using MoreMath.Application.UseCases.Authors.Commands;
using MoreMath.Application.UseCases.Authors.Queries;
using MoreMath.Shared.Result;
using System.Net;

namespace MoreMath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpPost("")]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateAuthor(CreateAuthorRequest request)
    {
        var res = await _mediator.Send(new CreateAuthorCommand(
            request.FirstName,
            request.LastName,
            request.AvatarUri,
            request.Info,
            request.ShortBio,
            request.Phone,
            request.Email,
            request.WhatsApp,
            request.Telegram,
            request.Website,
            request.Options));
        return res.ToHttpCreated($"/authors/{res.Value}");
    }

    [HttpGet("")]
    [ProducesResponseType<IEnumerable<AuthorDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetAuthors(string? firstName, string? lastName)
    {
        var res = await _mediator.Send(new GetAuthorsQuery(firstName, lastName));
        return res.ToHttpResult();
    }


    [HttpGet("{id}")]
    [ProducesResponseType<AuthorDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetAuthorById(int id)
    {
        var res = await _mediator.Send(new GetAuthorByIdQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpGet("{id}/articles")]
    [ProducesResponseType<ArticleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetAuthorArticles(int id)
    {
        var res = await _mediator.Send(new GetAuthorArticlesQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> UpdateAuthor(int id, UpdateAuthorRequest request)
    {
        var res = await _mediator.Send(new UpdateAuthorCommand(
            id,
            request.FirstName,
            request.LastName,
            request.AvatarUri,
            request.Info,
            request.ShortBio,
            request.Phone,
            request.Email,
            request.WhatsApp,
            request.Telegram,
            request.Website,
            request.Options));
        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.BadRequest);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAuthor(int id)
    {
        var res = await _mediator.Send(new DeleteAuthorCommand(id));

        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }
}
