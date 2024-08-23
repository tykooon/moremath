using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoreMath.Api.Extensions;
using MoreMath.Application.UseCases.Tags.Commands;
using MoreMath.Application.UseCases.Tags.Queries;
using MoreMath.Dto.Dtos;
using MoreMath.Shared.Result;
using System.Net;

namespace MoreMath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("")]
    [ProducesResponseType<IEnumerable<TagDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetTags(string? searchString)
    {
        var res = await _mediator.Send(new GetTagsQuery(searchString));
        return res.ToHttpResult();
    }

    [HttpGet("{id}")]
    [ProducesResponseType<TagDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetTagById(int id)
    {
        var res = await _mediator.Send(new GetTagByIdQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpPost("")]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateTag(string tagName)
    {
        var res = await _mediator.Send(new CreateTagCommand(tagName));
        return res.ToHttpCreated($"/tags/{res.Value}");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteTag(int id)
    {
        var res = await _mediator.Send(new DeleteTagCommand(id));

        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }

}
