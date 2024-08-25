using MoreMath.Dto.Dtos;
using MoreMath.Core.Entities;

namespace MoreMath.Dto.Mappers;

public static class ArticleMappers
{
    public static ArticleDto ToDto(this Article article) => new(
        article.Id,
        article.Title,
        article.Abstract,
        article.BodyUri?.ToString() ?? "",
        article.Authors.Select(a => a.Id).ToArray(),
        article.CategoryId,
        article.Category?.CategoryName ?? "",
        article.Tags.Select(t => t.TagName).ToArray(),
        article.Created,
        article.Modified);

    public static Article FromDto(this ArticleDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        Abstract = dto.Abstract,
        BodyUri = dto.BodyUri == null ? null : new Uri(Uri.EscapeDataString(dto.BodyUri), UriKind.RelativeOrAbsolute),
        CategoryId = dto.CategoryId
    };
}
