using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts.Services;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.Services;

public class TagService(AppDbContext context) : ITagService
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Tag>> CreateTagsFromNames(IEnumerable<string> tagList)
    {
        var existingTags = (await GetTagsByNames(tagList)).Select(t => t.TagName);

        var newTags = tagList.Where(name => !existingTags.Contains(name)).Select(name => new Tag() { TagName = name });

        await _context.AddRangeAsync(newTags);
        await _context.SaveChangesAsync();

        return newTags;
    }

    public async Task<IEnumerable<Tag>> GetTagsByNames(IEnumerable<string> tagList)
    {
        var existingTags = await _context.Tags.Where(t => tagList.Contains(t.TagName)).ToListAsync();
        return existingTags;
    }
}
