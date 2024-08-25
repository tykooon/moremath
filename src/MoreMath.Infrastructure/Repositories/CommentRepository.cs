using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts.Repositories;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.Repositories;

public class CommentRepository(DbContext context) : RepositoryWithDate<Comment, int>(context), ICommentRepository
{
    protected override IQueryable<Comment> MakeInclusions()
    {
        return base.MakeInclusions().Include(c => c.User);
    }

}
