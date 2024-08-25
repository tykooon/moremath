using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using MoreMath.Core.Entities;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record UpdateCommentToArticleCommand(
    int ArticleId,
    int CommentId,
    int? UserId,
    string? Text,
    bool? IsDeleted): IRequest<ResultWrap>;

public class UpdateCommentToArticleHandler(IUnitOfWork unitOfWork):
    AbstractHandler<UpdateCommentToArticleCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(UpdateCommentToArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(command.ArticleId);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var comment = await _unitOfWork.CommentRepo.FindAsync(command.CommentId);

        if(comment == null)
        {
            return ResultWrap.Failure(new Error("Comment.NotFound", "Failed to get comment with given Id."));
        }

        if (comment.ArticleId != command.ArticleId)
        {
            return ResultWrap.Failure(new Error("Article.WrongCommentId", "The article has no comment with provided Id."));
        }

        if (comment.User != null && comment.UserId != command.UserId)
        {
            return ResultWrap.Failure(new Error("Article.WrongUser", "The User with provided Id has no permission to edit this comment."));
        }

        if (comment.User == null && command.UserId != null)
        {
            var user = await _unitOfWork.UserRepo.FindAsync(command.UserId.Value);

            if(user == null)
            {
                return ResultWrap.Failure(new Error("User.NotFound", "The User with provided Id was not found."));
            }

            comment.User = user;
        }

        comment.Text = command.Text ?? comment.Text;
        comment.IsDeleted = command.IsDeleted ?? comment.IsDeleted;

        _unitOfWork.CommentRepo.Update(comment);
        await _unitOfWork.CommitAsync();

        return ResultWrap.Success();
    }
}
