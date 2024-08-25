using MoreMath.Core.Entities;

namespace MoreMath.Application.Contracts.Services;

public interface ITagService
{
    Task<IEnumerable<Tag>> GetTagsByNamesAsync(IEnumerable<string> tagList);
    Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> tagList);
    Task<IEnumerable<Tag>> CreateTagsFromNamesAsync(IEnumerable<string> tagList);
}
