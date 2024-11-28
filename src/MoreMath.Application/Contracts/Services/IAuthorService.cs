using MoreMath.Core.Entities;
using MoreMath.Dto.Dtos;

namespace MoreMath.Application.Contracts.Services;

public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAuthorsByIdsAsync(IEnumerable<int> ids);
    Task<IEnumerable<AuthorDto>> GetAuthorsAsync();
}
