using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.Contracts.Services;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Core.Entities;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record CreateArticleCommand(
    string Title,
    string Abstract,
    string BodyUri,
    string ImageUri,
    string Slug,
    int[] AuthorsId,
    int CategoryId,
    string[] Tags) : IRequest<ResultWrap<int>>;



public class CreateArticleHandler(IUnitOfWork unitOfWork, IAuthorService authorService, ITagService tagService):
    AbstractHandler<CreateArticleCommand, ResultWrap<int>>(unitOfWork)
{
    private readonly IAuthorService _authorService = authorService;
    private readonly ITagService _tagService = tagService;

    public override async Task<ResultWrap<int>> Handle(CreateArticleCommand command, CancellationToken cancellationToken)
    {
        var authorList = await _authorService.GetAuthorsByIdsAsync(command.AuthorsId);
        var category = await _unitOfWork.CategoryRepo.FindAsync(command.CategoryId);
        var tags = await _tagService.GetTagsByNamesAsync(command.Tags);


        Article article = new()
        {
            Title = command.Title,
            Abstract = command.Abstract,
            BodyUri = command.BodyUri,
            ImageUri = command.ImageUri,
            Slug = command.Slug,
            Authors = authorList.ToList(),
            Category = category,
            Tags = tags.ToList(), 
        };

        await _unitOfWork.ArticleRepo.AddAsync(article);
        await _unitOfWork.CommitAsync();
        return article.Id == 0
            ? ResultWrap.Failure(new Error("Article.CreateError", "Article was not created"))
            : ResultWrap<int>.Success(article.Id);
    }
}
