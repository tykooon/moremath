using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts.Repositories;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.Repositories;

public class TestLessonRepository(DbContext context) : RepositoryWithDate<TestLessonOrder, int>(context), ITestLessonRepository
{
}
