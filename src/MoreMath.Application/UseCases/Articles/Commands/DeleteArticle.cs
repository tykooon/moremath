using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record DeleteArticleCommand(int Id) : IRequest<ResultWrap>;



public class  DeleteArticleHandler(IUnitOfWork unitOfWork):
    AbstractHandler<DeleteArticleCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(DeleteArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(command.Id);

        if (article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to delete author with given Id. Author wasn't found."));
        }

        _unitOfWork.ArticleRepo.Delete(article);

        await _unitOfWork.CommitAsync();
        return ResultWrap.Success();
    }
}
