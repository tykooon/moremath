using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record AddAuthorToArticleCommand(
    int ArticleId,
    int? AuthorId,
    string? FirstName,
    string? LastName): IRequest<ResultWrap>;

public class AddAuthorToArticleHandler(IUnitOfWork unitOfWork):
    AbstractHandler<AddAuthorToArticleCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(AddAuthorToArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(command.ArticleId);

        if(article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to get article with given Id."));
        }

        var authors = await _unitOfWork.AuthorRepo.GetFilteredAsync(a =>
            (command.AuthorId == null || a.Id == command.AuthorId) &&
            (command.FirstName == null || a.FirstName == command.FirstName) &&
            (command.LastName == null || a.LastName == command.LastName));

        if (!authors.Any())
        {
            return ResultWrap.Failure(new Error("Author.NotFound", "Failed to get author with provided data."));
        }

        if (authors.Count() > 1)
        {
            return ResultWrap.Failure(new Error("Author.NotSpecified", "Sorry, several authors match provided data. Try to specift it."));
        }

        var author = authors.First();

        if (article.Authors.Contains(author))
        {
            return ResultWrap.Success();
        }

        article.Authors.Add(author);
        await _unitOfWork.CommitAsync();

        return ResultWrap.Success();
    }
}
