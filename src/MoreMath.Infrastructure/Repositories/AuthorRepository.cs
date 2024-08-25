using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts.Repositories;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.Repositories;

public class AuthorRepository(DbContext context) : RepositoryWithDate<Author, int>(context), IAuthorRepository
{
    protected override IQueryable<Author> MakeInclusions()
    {
        return base.MakeInclusions();
    }
}
