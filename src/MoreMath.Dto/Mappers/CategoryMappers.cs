using MoreMath.Dto.Dtos;
using MoreMath.Core.Entities;

namespace MoreMath.Dto.Mappers;

public static class CategoryMappers
{
    public static CategoryDto ToDto(this Category category) => new(
        category.Id,
        category.CategoryName,
        category.Description);

    public static Category FromDto(this CategoryDto dto) => new()
    {
        Id = dto.Id,
        CategoryName = dto.CategoryName,
        Description = dto.Description
    };
}
