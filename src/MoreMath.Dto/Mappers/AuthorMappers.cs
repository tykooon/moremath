using MoreMath.Dto.Dtos;
using MoreMath.Core.Entities;
using System.Numerics;

namespace MoreMath.Dto.Mappers;

public static class AuthorMappers
{
    public static AuthorDto ToDto(this Author author) => new(
        author.Id,
        author.FirstName,
        author.LastName,
        author.SlugName,
        author.Info,
        author.ShortBio,
        author.Phone,
        author.Email,
        author.WhatsApp,
        author.Telegram,
        author.Website,
        author.Options,
        author.Created,
        author.Modified);

    public static Author FromDto(this AuthorDto dto) => new()
    {
        Id = dto.Id,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        SlugName = dto.SlugName,
        Info = dto.Info,
        ShortBio = dto.ShortBio
    };
}
