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
    public async Task<IResult> GetArticles(
        string? category,
        int? authorId,
        string? firstName,
        string? lastName,
        [FromQuery] string[]? tags)
    {
        var res = await _mediator.Send(new GetArticlesQuery(category, authorId, firstName, lastName, tags));
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
            request.ImageUri,
            request.Slug,
            request.AuthorsId,
            request.CategoryId,
            request.Tags));
        return res.ToHttpCreated($"/articles/{res.Value}");
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<ArticleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetArticleById(int id)
    {
        var res = await _mediator.Send(new GetArticleByIdQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpGet("{slug:minlength(8)}")]
    [ProducesResponseType<ArticleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetArticleBySlug(string slug)
    {
        var res = await _mediator.Send(new GetArticleBySlugQuery(slug));
        return res.ToHttpNotFound();
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
            request.ImageUri,
            request.Slug,
            request.CategoryId));
        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.BadRequest);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteArticle(int id)
    {
        var res = await _mediator.Send(new DeleteArticleCommand(id));
        
        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }

    [HttpGet("{id}/authors")]
    [ProducesResponseType<IEnumerable<ArticleDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetArticleAuthors(int id)
    {
        var res = await _mediator.Send(new GetArticleAuthorsQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpPost("{id}/authors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> AddAuthorToArticle(int id, [FromBody] AddAuthorToArticleRequest request)
    {
        var res = await _mediator.Send(new AddAuthorToArticleCommand(id, request.AuthorId, request.FirstName, request.LastName));
        return res.ToOkOrNotFound();
    }

    [HttpDelete("{id}/authors/{authorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAuthorFromArticle(int id, int authorId)
    {
        var res = await _mediator.Send(new DeleteAuthorFromArticleCommand(id, authorId));
        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }

    [HttpGet("{id}/tags")]
    [ProducesResponseType<IEnumerable<TagDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetArticleTags(int id)
    {
        var res = await _mediator.Send(new GetArticleTagsQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpPost("{id}/tags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> AddTagsToArticle(int id, [FromBody] AddTagsToArticleRequest request)
    {
        var res = await _mediator.Send(new AddTagsToArticleCommand(id, request.TagsId, request.TagNames));
        return res.ToOkOrNotFound();
    }

    [HttpDelete("{id}/tags/{tagId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteTagFromArticle(int id, int tagId)
    {
        var res = await _mediator.Send(new DeleteTagFromArticleCommand(id, tagId));
        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }

    [HttpGet("{id}/comments")]
    [ProducesResponseType<IEnumerable<CommentDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetArticleComments(int id)
    {
        var res = await _mediator.Send(new GetArticleCommentsQuery(id));
        return res.ToHttpNotFound();
    }

    [HttpPost("{id}/comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> AddCommentToArticle(int id, [FromBody] AddCommentToArticleRequest request)
    {
        var res = await _mediator.Send(new AddCommentToArticleCommand(id, request.UserId, request.Text));
        return res.ToOkOrNotFound();
    }

    [HttpPut("{id}/comments/{commentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> UpdateCommentToArticle(int id, int commentId, [FromBody] UpdateCommentToArticleRequest request)
    {
        var res = await _mediator.Send(new UpdateCommentToArticleCommand(id, commentId, request.UserId, request.Text, request.IsDeleted));
        return res.ToHttp(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }

    [HttpDelete("{id}/comments/{commentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error[]>(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteCommentToArticle(int id, int commentId)
    {
        var res = await _mediator.Send(new DeleteCommentToArticleCommand(id, commentId));
        return res.ToHttp(HttpStatusCode.OK, HttpStatusCode.NoContent);
    }

}