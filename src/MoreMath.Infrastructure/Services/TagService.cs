using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts.Services;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.Services;

public class TagService(AppDbContext context) : ITagService
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Tag>> CreateTagsFromNamesAsync(IEnumerable<string> tagList)
    {
        var existingTags = (await GetTagsByNamesAsync(tagList)).Select(t => t.TagName);

        var newTags = tagList.Where(name => !existingTags.Contains(name)).Select(name => new Tag() { TagName = name });

        await _context.AddRangeAsync(newTags);
        await _context.SaveChangesAsync();

        return newTags;
    }

    public async Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> tagList)
    {
        var existingTags = await _context.Tags.Where(t => tagList.Contains(t.Id)).ToListAsync();
        return existingTags;
    }

    public async Task<IEnumerable<Tag>> GetTagsByNamesAsync(IEnumerable<string> tagList)
    {
        var existingTags = await _context.Tags.Where(t => tagList.Contains(t.TagName)).ToListAsync();
        return existingTags;
    }
}
