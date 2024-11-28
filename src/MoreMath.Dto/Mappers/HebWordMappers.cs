using MoreMath.Dto.Dtos;
using MoreMath.Core.Entities;
using MoreMath.Dto.Responses;

namespace MoreMath.Dto.Mappers;

public static class HebWordMappers
{
    public static HebWordDto ToDto(this HebWord word) => new(
        word.Id,
        word.Shoresh,
        word.Niqqud,
        word.NoNiqqud,
        word.ExtraForm,
        word.Spelling,
        word.StressLetter,
        word.Translation,
        word.Notes,
        word.ExtraInfo,
        word.Tags.Select(t => t.TagName).ToArray());

}
