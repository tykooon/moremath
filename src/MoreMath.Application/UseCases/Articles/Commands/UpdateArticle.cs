using MediatR;
using MoreMath.Application.Contracts;
using MoreMath.Application.Contracts.Services;
using MoreMath.Application.UseCases.Abstracts;
using MoreMath.Shared.Result;

namespace MoreMath.Application.UseCases.Articles.Commands;

public record UpdateArticleCommand(
    int Id,
    string? Title,
    string? Abstract,
    string? BodyUri,
    int? CategoryId) : IRequest<ResultWrap>;



public class  UpdateArticleHandler(IUnitOfWork unitOfWork):
    AbstractHandler<UpdateArticleCommand, ResultWrap>(unitOfWork)
{
    public override async Task<ResultWrap> Handle(UpdateArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(command.Id);

        if (article == null)
        {
            return ResultWrap.Failure(new Error("Article.NotFound", "Failed to update article with given Id. Article wasn't found."));
        }

        article.Title = command.Title ?? article.Title;
        article.Abstract = command.Abstract ?? article.Abstract;
        article.BodyUri = command.BodyUri != null ? new Uri(command.BodyUri) : article.BodyUri;
        if (command.CategoryId != null)
        {
            var category = await _unitOfWork.CategoryRepo.FindAsync(command.CategoryId.Value);
            if (category == null)
            {
                return ResultWrap.Failure(new Error("Category.NotFound", "Failed to update article. Provided CategoryId was not found. Article wasn't found."));
            }
            article.Category = category;
        }

        _unitOfWork.ArticleRepo.Update(article);

        await _unitOfWork.CommitAsync();
        return ResultWrap.Success();
    }
}
