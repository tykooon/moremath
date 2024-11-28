using MediatR;
using MoreMath.Core.Entities;
using MoreMath.Shared.Models;

namespace MoreMath.Application.Contracts.Services;

public interface IHebWordService
{
    Task<IEnumerable<HebWord>> GetHebWordsByTagsAsync(IEnumerable<Tag> tags);
    (IQueryable<HebWord>, int) GetHebWordsByFilter(HebWordFilter filter, int start, int take);
}
