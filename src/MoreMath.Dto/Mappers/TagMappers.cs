using MoreMath.Dto.Dtos;
using MoreMath.Core.Entities;

namespace MoreMath.Dto.Mappers;

public static class TagMappers
{
    public static TagDto ToDto(this Tag tag) => new(
        tag.Id,
        tag.TagName);

    public static Tag FromDto(this TagDto dto) => new()
    {
        Id = dto.Id,
        TagName = dto.TagName
    };
}
