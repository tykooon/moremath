﻿using Microsoft.EntityFrameworkCore;
using MoreMath.Application.Contracts.Repositories;
using MoreMath.Core.Entities;

namespace MoreMath.Infrastructure.Repositories;

public class TagRepository(DbContext context) : Repository<Tag, int>(context), ITagRepository
{
}
