using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record DeleteCommentToArticleCommand(
    int ArticleId,
    int CommentId): IRequest<ResultWrap>;

public class DeleteCommentToArticleHandler(IUnitOfWork unitOfWork):
    AbstractHandler<DeleteCommentToArticleCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(DeleteCommentToArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(command.ArticleId);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var comment = await _unitOfWork.CommentRepo.FindAsync(command.CommentId);

        if (comment == null)
        {
            return ResultWrap.Failure(new Error("Comment.NotFound", "Failed to get comment with provided Id."));
        }

        if (comment.ArticleId != command.ArticleId)
        {
            return ResultWrap.Failure(new Error("Article.WrongCommentId", "The article has no comment with provided Id."));
        }


        _unitOfWork.CommentRepo.Delete(comment);
        await _unitOfWork.CommitAsync();

        return ResultWrap.Success();
    }
}
