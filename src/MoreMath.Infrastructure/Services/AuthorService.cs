using MoreMath.Application.Contracts;
using MoreMath.Application.Contracts.Services;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.Services;

public class AuthorService(IUnitOfWork unitOfWork) : IAuthorService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<Author>> GetAuthorsByIdsAsync(IEnumerable<int> ids)
    {
        var authorList = await _unitOfWork.AuthorRepo.GetFilteredAsync(a => ids.Contains(a.Id));
        return authorList;
    }
}
