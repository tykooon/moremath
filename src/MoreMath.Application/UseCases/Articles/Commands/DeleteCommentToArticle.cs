using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record DeleteCommentToArticleCommand(
    int ArticleId,
    int CommentId): IRequest<ResultWrap>;

public class DeleteCommentToArticleHandler(IAppDbContext context):
    AbstractHandler<DeleteCommentToArticleCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(DeleteCommentToArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.Include(a => a.Comments).FirstOrDefaultAsync(a => a.Id == command.ArticleId, cancellationToken);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var comment = await _context.Comments.FindAsync(command.CommentId);

        if (comment == null)
        {
            return ResultWrap.Failure(new Error("Comment.NotFound", "Failed to get comment with provided Id."));
        }

        if (comment.ArticleId != command.ArticleId)
        {
            return ResultWrap.Failure(new Error("Article.WrongCommentId", "The article has no comment with provided Id."));
        }


        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return ResultWrap.Success();
    }
}
