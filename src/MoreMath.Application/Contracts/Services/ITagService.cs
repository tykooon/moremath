using MoreMath.Core.Entities;

namespace MoreMath.Application.Contracts.Services;

public interface ITagService
{
    Task<IEnumerable<Tag>> GetTagsByNames(IEnumerable<string> tagList);
    Task<IEnumerable<Tag>> CreateTagsFromNames(IEnumerable<string> tagList);
}
