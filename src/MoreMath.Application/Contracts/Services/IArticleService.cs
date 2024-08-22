using MoreMath.Core.Entities;

namespace MoreMath.Application.Contracts.Services;

public interface IArticleService
{
    Task AddAuthorToArticle(int articleId, int authorId);
    Task RemoveAuthorFromArticle(int articleId, int authorId);
}
