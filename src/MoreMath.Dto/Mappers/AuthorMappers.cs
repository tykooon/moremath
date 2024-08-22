using MoreMath.Dto.Dtos;
using MoreMath.Core.Entities;

namespace MoreMath.Dto.Mappers;

public static class AuthorMappers
{
    public static AuthorDto ToDto(this Author author) => new(
        author.Id,
        author.FirstName,
        author.LastName,
        author.Avatar?.ToString(),
        author.Info,
        author.ShortBio,
        author.Created,
        author.Modified);

    public static Author FromDto(this AuthorDto dto) => new()
    {
        Id = dto.Id,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Avatar = dto.AvatarUrl == null ? null : new Uri(Uri.EscapeDataString(dto.AvatarUrl), UriKind.RelativeOrAbsolute),
        Info = dto.Info,
        ShortBio = dto.ShortBio
    };
}
