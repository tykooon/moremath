using MoreMath.Dto.Dtos;
using MoreMath.Core.Entities;

namespace MoreMath.Dto.Mappers;

public static class UserMappers
{
    public static UserDto ToDto(this User user) => new(
        user.Id,
        user.Username,
        user.IsActive,
        user.AuthorId,
        user.Created,
        user.Modified);

    public static User FromDto(this UserDto dto) => new()
    {
        Id = dto.Id,
        Username = dto.Username,
        IsActive = dto.IsActive,
        AuthorId = dto.AuthorId
    };
}
