using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts.Services;
using MoreMath.Core.Entities;
using MoreMath.Shared.Models;
namespace MoreMath.Infrastructure.Services;

public class HebWordService(AppDbContext context) : IHebWordService
{
    private readonly AppDbContext _context = context;

    public (IQueryable<HebWord>, int) GetHebWordsByFilter(HebWordFilter filter, int start, int take)
    {
        var query = from word in _context.HebWords
                    where (string.IsNullOrEmpty(filter.Translation) ||
                        EF.Functions.Like(word.Translation, $"%{filter.Translation}%")) &&
                    (string.IsNullOrEmpty(filter.Shoresh) ||
                        EF.Functions.Like(word.Shoresh, $"%{filter.Shoresh}%")) &&
                    (string.IsNullOrEmpty(filter.Word) ||
                        EF.Functions.Like(word.NoNiqqud, $"%{filter.Word}%"))
                    select word;

        if (filter.Tags != null && filter.Tags.Length > 0)
        {
            var query2 = from word in query
                         where word.Tags.Any(t => filter.Tags.Contains(t.TagName))
                         select word;

            var res2 = query2
                .AsNoTracking()
                .OrderBy(w => w.Niqqud)
                .Skip(start)
                .Take(take).Include(w => w.Tags);
                
            return (res2, query2.Count());
        }

        var res = query.AsNoTracking().OrderBy(w => w.Niqqud).Skip(start).Take(take).Include(w => w.Tags);
        return (res, query.Count()); ;
    }

    public async Task<IEnumerable<HebWord>> GetHebWordsByTagsAsync(IEnumerable<Tag> tags)
    {
        var tagNames = tags.Select(t => t.TagName).ToList();
        var query = from tag in _context.Tags where tagNames.Contains(tag.TagName)
                  from word in _context.HebWords where word.Tags.Contains(tag)
                  select word;
        var res = await query.ToListAsync();
        return res;
    }
}
