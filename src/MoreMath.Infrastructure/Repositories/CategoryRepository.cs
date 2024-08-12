using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.Repositories;

public class CategoryRepository(DbContext context) : Repository<Category, int>(context), ICategoryRepository
{
}
