using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoreMath.Api.Extensions;
using MoreMath.Api.Requests.Authors;
using MoreMath.Application.Dtos;
using MoreMath.Application.UseCases.Authors.Commands;
using MoreMath.Application.UseCases.Authors.Queries;
using MoreMath.Shared.Result;

namespace MoreMath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : BaseApiController
{
    public AuthorsController(IMediator mediator) : base(mediator) { }

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
            request.ShortBio));
        return res.ToHttpCreated($"/authors/{res.Value}");
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
            request.ShortBio));
        return res.ToHttpResult();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAuthor(int id)
    {
        var res = await _mediator.Send(new DeleteAuthorCommand(id));
        return res.ToHttpNotFound();
    }

}
