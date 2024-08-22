using MoreMath.Application.Contracts;
using MoreMath.Application.Contracts.Services;

namespace MoreMath.Infrastructure.Services;

public class ArticleService(IUnitOfWork unitOfWork) : IArticleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task AddAuthorToArticle(int articleId, int authorId)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(articleId);
        if (article == null)
        {
            return;
        }

        var author = article.Authors.SingleOrDefault(a => a.Id == authorId);
        if(author != null && !article.Authors.Any(a => a.Id == authorId))
        {
            article.Authors.Add(author);
            await _unitOfWork.CommitAsync();
        }      
    }

    public async Task RemoveAuthorFromArticle(int articleId, int authorId)
    {
        var article = await _unitOfWork.ArticleRepo.FindAsync(articleId);
        if (article == null)
        {
            return;
        }

        var author = article.Authors.SingleOrDefault(a => a.Id == authorId);

        if (author != null && article.Authors.Any(a => a.Id == authorId))
        {
            article.Authors.Remove(author);
            await _unitOfWork.CommitAsync();
        }
    }
}
