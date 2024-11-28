using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoreMath.Api.Extensions;
using MoreMath.Dto.Dtos;
using MoreMath.Shared.Result;
using MoreMath.Application.UseCases.HebWords.Queries;
using MoreMath.Api.Requests.HebWords;
using MoreMath.Application.UseCases.HebWords.Commands;
using System.Net;
using MoreMath.Dto.Responses;

namespace MoreMath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HebWordsController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("")]
    [ProducesResponseType<IEnumerable<HebWordDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetHebWords(
        string? shoresh,
        string? noNiqqud,
        string? translation,
        [FromQuery] string[]? tags,
        bool hasAllTags)
    {
        var res = await _mediator.Send(new GetHebWordsQuery(shoresh, noNiqqud, translation, tags, hasAllTags));
        return res.ToHttpResult();
    }

    [HttpGet("paged")]
    [ProducesResponseType<IEnumerable<HebWordPageItem>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetHebWordsInfo(
        string? shoresh,
        string? noNiqqud,
        string? translation,
        [FromQuery] string[]? tags,
        bool hasAllTags,
        string? orderBy,
        bool descending,
        int start = 0,
        int take = 10)
    {
        var res = await _mediator.Send(new GetHebWordsPagedQuery()
        {
            Shoresh = shoresh,
            NoNiqqud = noNiqqud,
            Translation = translation,
            TagList = tags,
            HasAllTags = hasAllTags,
            OrderBy = orderBy,
            Decsending = descending,
            Start = start,
            Take = take
        });
        return res.ToHttpResult();
    }

    [HttpPost("")]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateArticle(CreateHebWordRequest request)
    {
        var res = await _mediator.Send(new CreateHebWordCommand(
            request.Shoresh,
            request.Niqqud,
            request.NoNiqqud,
            request.ExtraForm,
            request.Spelling,
            request.StressLetter,
            request.Translation,
            request.Notes,
            request.ExtraInfo,
            request.Tags));
        return res.ToHttpCreated($"/hebwords/{res.Value}");
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<ArticlePageItem>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetHebWordById(int id)
    {
        var res = await _mediator.Send(new GetHebWordByIdQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> UpdateHebWod(int id, UpdateHebWordRequest request)
    {
        var res = await _mediator.Send(new UpdateHebWordCommand(
            id,
            request.Shoresh,
            request.Niqqud,
            request.NoNiqqud,
            request.ExtraForm,
            request.Spelling,
            request.StressLetter,
            request.Translation,
            request.Notes,
            request.ExtraInfo));
        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.BadRequest);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteHebWord(int id)
    {
        var res = await _mediator.Send(new DeleteHebWordCommand(id));

        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }

    [HttpGet("{id}/tags")]
    [ProducesResponseType<IEnumerable<TagDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetArticleTags(int id)
    {
        var res = await _mediator.Send(new GetHebWordTagsQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpPost("{id}/tags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> AddTagsToHebWord(int id, [FromBody] AddTagsToHebWordRequest request)
    {
        var res = await _mediator.Send(new AddTagsToHebWordCommand(id, request.TagsId, request.TagNames));
        return res.ToOkOrNotFound();
    }

    [HttpDelete("{id}/tags/{tagId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteTagFromArticle(int id, int tagId)
    {
        var res = await _mediator.Send(new DeleteTagFromHebWordCommand(id, tagId));
        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }
}