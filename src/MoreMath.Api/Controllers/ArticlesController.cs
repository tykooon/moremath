using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoreMath.Api.Extensions;
using MoreMath.Dto.Dtos;
using MoreMath.Shared.Result;
using MoreMath.Application.UseCases.Articles.Queries;
using MoreMath.Api.Requests.Articles;
using MoreMath.Application.UseCases.Articles.Commands;
using System.Net;

namespace MoreMath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController : BaseApiController
{
    public ArticlesController(IMediator mediator) : base(mediator) { }

    [HttpGet("")]
    [ProducesResponseType<IEnumerable<ArticleDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetArticles(string? category, [FromQuery] string[]? tags)
    {
        var res = await _mediator.Send(new GetArticlesQuery(category, tags));
        return res.ToHttpResult();
    }

    [HttpGet("{id}")]
    [ProducesResponseType<ArticleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetArticleById(int id)
    {
        var res = await _mediator.Send(new GetArticleByIdQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpGet("author")]
    [ProducesResponseType<IEnumerable<ArticleDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetArticlesByAuthor(int? authorId, string? firstName, string? lastName)
    {
        var res = await _mediator.Send(new GetArticlesByAuthorQuery(authorId, firstName, lastName));
        return res.ToHttpResult();
    }

    [HttpPost("")]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateArticle(CreateArticleRequest request)
    {
        var res = await _mediator.Send(new CreateArticleCommand(
            request.Title,
            request.Abstract,
            request.BodyUri,
            request.AuthorsId,
            request.CategoryId,
            request.Tags));
        return res.ToHttpCreated($"/articles/{res.Value}");
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> UpdateArticle(int id, UpdateArticleRequest request)
    {
        var res = await _mediator.Send(new UpdateArticleCommand(
            id,
            request.Title,
            request.Abstract,
            request.BodyUri,
            request.CategoryId));
        return res.ToHttpResult();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteArticle(int id)
    {
        var res = await _mediator.Send(new DeleteArticleCommand(id));
        
        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }
}
