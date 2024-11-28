using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Application.Contracts.Services;

namespace MoreMath.Infrastructure.Services;

public class ArticleService(IAppDbContext context) : IArticleService
{
    private readonly IAppDbContext _context = context;

    public async Task AddAuthorToArticle(int articleId, int authorId)
    {
        var article = await _context.Articles.Include(a => a.Authors).FirstOrDefaultAsync(ar => ar.Id.Equals(articleId));
        if (article == null)
        {
            return;
        }

        var author = article.Authors.SingleOrDefault(a => a.Id == authorId);
        if(author != null && !article.Authors.Any(a => a.Id == authorId))
        {
            article.Authors.Add(author);
            await _context.SaveChangesAsync();
        }      
    }

    public async Task RemoveAuthorFromArticle(int articleId, int authorId)
    {
        var article = await _context.Articles.Include(a => a.Authors).FirstOrDefaultAsync(ar => ar.Id.Equals(articleId));
        if (article == null)
        {
            return;
        }

        var author = article.Authors.SingleOrDefault(a => a.Id == authorId);

        if (author != null && article.Authors.Any(a => a.Id == authorId))
        {
            article.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }
}
