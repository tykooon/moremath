using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts.Services;
using MoreMath.Core.Entities;
using MoreMath.Dto.Dtos;
using MoreMath.Dto.Mappers;

namespace MoreMath.Infrastructure.Services;

public class AuthorService(AppDbContext context) : IAuthorService
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<AuthorDto>> GetAuthorsAsync()
    {
        var authorList = await _context.Authors.AsNoTracking().ToListAsync();
        return authorList.Select(a => a.ToDto());
    }

    public async Task<IEnumerable<Author>> GetAuthorsByIdsAsync(IEnumerable<int> ids)
    {
        var authorList = await _context.Authors.Where(a => ids.Contains(a.Id)).ToListAsync();
        return authorList;
    }
}
