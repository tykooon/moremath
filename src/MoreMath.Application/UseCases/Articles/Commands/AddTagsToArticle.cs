using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.Contracts.Services;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record AddTagsToArticleCommand(
    int ArticleId,
    int[]? TagsId,
    string[]? TagNames): IRequest<ResultWrap>;

public class AddTagsToArticleHandler(IUnitOfWork unitOfWork, ITagService tagService):
    AbstractHandler<AddTagsToArticleCommand, ResultWrap>(unitOfWork)
{
    private readonly ITagService _tagService = tagService;

    public override async Task<ResultWrap> Handle(AddTagsToArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(command.ArticleId);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var tagsById = command.TagsId != null
            ? await _tagService.GetTagsByIdsAsync(command.TagsId)
            : [];

        var tagsByName = command.TagNames != null
            ? await _tagService.GetTagsByNamesAsync(command.TagNames)
            : [];

        var tags = tagsById.ToList();
        tags.AddRange(tagsByName);

        if (tags.Count == 0)
        {
            return ResultWrap.Failure(new Error("Tags.NotFound", "Failed to get Tags with provided data."));
        }

        foreach(var tag in tags)
        {
            article.Tags.Add(tag);
        }

        article.UpdateTimeMark();

        await _unitOfWork.CommitAsync();

        return ResultWrap.Success();
    }
}
