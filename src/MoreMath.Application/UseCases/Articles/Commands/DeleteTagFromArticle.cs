using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record DeleteTagFromArticleCommand(
    int ArticleId,
    int TagId): IRequest<ResultWrap>;

public class DeleteTagFromArticleHandler(IUnitOfWork unitOfWork):
    AbstractHandler<DeleteTagFromArticleCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(DeleteTagFromArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(command.ArticleId);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var Tag = await _unitOfWork.TagRepo.FindAsync(command.TagId);

        if (Tag == null)
        {
            return ResultWrap.Failure(new Error("Tag.NotFound", "Failed to get Tag with provided Id."));
        }

        article.Tags.Remove(Tag);
        article.UpdateTimeMark();

        await _unitOfWork.CommitAsync();

        return ResultWrap.Success();
    }
}
