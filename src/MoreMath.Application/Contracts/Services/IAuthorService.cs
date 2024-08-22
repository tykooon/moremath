using MoreMath.Core.Entities;

namespace MoreMath.Application.Contracts.Services;

public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAuthorsByIdsAsync(IEnumerable<int> ids);
}
