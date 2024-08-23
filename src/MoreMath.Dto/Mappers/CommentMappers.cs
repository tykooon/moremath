using MoreMath.Dto.Dtos;
using MoreMath.Core.Entities;

namespace MoreMath.Dto.Mappers;

public static class CommentMappers
{
    public static CommentDto ToDto(this Comment comment) => new(
        comment.Id,
        comment.UserId,
        comment.ArticleId,
        comment.Text,
        comment.IsDeleted,
        comment.Created,
        comment.Modified);

    public static Comment FromDto(this CommentDto dto) => new()
    {
        Id = dto.Id,
        Text = dto.Text,
        IsDeleted = dto.IsDeleted
    };
}
