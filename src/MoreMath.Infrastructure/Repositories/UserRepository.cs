using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.Repositories;

public class UserRepository(DbContext context) : RepositoryWithDate<User, int>(context), IUserRepository
{
}
