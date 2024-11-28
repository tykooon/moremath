namespace MoreMath.Dto.Responses;

public record HebWordPageItem(
    int Id,
    string Shoresh,
    string Niqqud,
    string NoNiqqud,
    string ExtraForm,
    string Spelling,
    int StressLetter,
    string Translation,
    string Notes,
    string ExtraInfo,
    string? TagsConcat,
    int Total);
