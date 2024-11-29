using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts.Services;
using MoreMath.Core.Entities;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;

namespace MoreMath.Infrastructure.Services;

public class AuthorService(IDbContextFactory<AppDbContext> factory) : IAuthorService
{
    private readonly IDbContextFactory<AppDbContext> _factory = factory;

    public async Task<IEnumerable<AuthorDto>> GetAuthorsAsync()
    {
        using (var context = _factory.CreateDbContext())
        {
            var authorList = await context.Authors.AsNoTracking().ToListAsync();
            return authorList.Select(a => a.ToDto());
        }
    }

    public async Task<IEnumerable<Author>> GetAuthorsByIdsAsync(IEnumerable<int> ids)
    {
        using (var context = _factory.CreateDbContext())
        {
            var authorList = await context.Authors.Where(a => ids.Contains(a.Id)).ToListAsync();
            return authorList;
        }
    }
}
