using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record DeleteAuthorFromArticleCommand(
    int ArticleId,
    int AuthorId): IRequest<ResultWrap>;

public class DeleteAuthorFromArticleHandler(IUnitOfWork unitOfWork):
    AbstractHandler<DeleteAuthorFromArticleCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(DeleteAuthorFromArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(command.ArticleId);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var author = await _unitOfWork.AuthorRepo.FindAsync(command.AuthorId);

        if (author == null)
        {
            return ResultWrap.Failure(new Error("Author.NotFound", "Failed to get author with provided Id."));
        }

        article.Authors.Remove(author);
        article.UpdateTimeMark();

        await _unitOfWork.CommitAsync();

        return ResultWrap.Success();
    }
}
