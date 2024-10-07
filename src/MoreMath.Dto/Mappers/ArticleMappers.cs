using MoreMath.Dto.Dtos;
using MoreMath.Core.Entities;

namespace MoreMath.Dto.Mappers;

public static class ArticleMappers
{
    public static ArticleDto ToDto(this Article article) => new(
        article.Id,
        article.Title,
        article.Abstract,
        article.BodyUri,
        article.ImageUri,
        article.Slug,
        article.Authors.Select(a => a.Id).ToArray(),
        article.CategoryId ?? 0,
        article.Category?.CategoryName ?? "",
        article.Tags.Select(t => t.TagName).ToArray(),
        article.Created,
        article.Modified);

    // TODO Check, if any FromDto Mapper is really needed / used

    public static Article FromDto(this ArticleDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        Abstract = dto.Abstract,
        BodyUri = dto.BodyUri,
        ImageUri = dto.ImageUri,
        Slug = dto.Slug,
        CategoryId = dto.CategoryId == 0 ? null : dto.CategoryId
    };
}
