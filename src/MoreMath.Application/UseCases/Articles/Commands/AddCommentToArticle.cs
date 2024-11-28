using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;
using MoreMath.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record AddCommentToArticleCommand(
    int ArticleId,
    int? UserId,
    string Text): IRequest<ResultWrap>;

public class AddCommentToArticleHandler(IAppDbContext context):
    AbstractHandler<AddCommentToArticleCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(AddCommentToArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.Include(a => a.Comments).FirstOrDefaultAsync(a => a.Id == command.ArticleId, cancellationToken);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var user = command.UserId == null
            ? null
            : await _context.Users.FindAsync([command.UserId.Value], cancellationToken);

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

        await _context.SaveChangesAsync(cancellationToken);

        return ResultWrap.Success();
    }
}
