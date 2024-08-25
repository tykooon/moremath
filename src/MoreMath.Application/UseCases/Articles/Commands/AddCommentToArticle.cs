using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using MoreMath.Core.Entities;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record AddCommentToArticleCommand(
    int ArticleId,
    int? UserId,
    string Text): IRequest<ResultWrap>;

public class AddCommentToArticleHandler(IUnitOfWork unitOfWork):
    AbstractHandler<AddCommentToArticleCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(AddCommentToArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(command.ArticleId);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var user = command.UserId == null
            ? null
            : await _unitOfWork.UserRepo.FindAsync(command.UserId.Value);

        if (command.UserId != null && user == null)
        {
            return ResultWrap.Failure(new Error("User.NotFound", "Failed to get user with given Id."));
        }

        var comment = new Comment()
        {
            Article = article,
            Text = command.Text,
            User = user
        };

        article.Comments.Add(comment);

        await _unitOfWork.CommitAsync();

        return ResultWrap.Success();
    }
}
