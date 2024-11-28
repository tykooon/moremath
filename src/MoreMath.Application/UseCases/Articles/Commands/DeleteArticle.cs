using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record DeleteArticleCommand(int Id) : IRequest<ResultWrap>;

public class  DeleteArticleHandler(IAppDbContext context):
    AbstractHandler<DeleteArticleCommand, ResultWrap>(context)
{
    public override async Task<ResultWrap> Handle(DeleteArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.FindAsync([command.Id], cancellationToken: cancellationToken);

        if (article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to delete article with given Id. Article wasn't found."));
        }

        _context.Articles.Remove(article);

        await _context.SaveChangesAsync(cancellationToken);
        return ResultWrap.Success();
    }
}
