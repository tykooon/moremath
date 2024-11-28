namespace MoreMath.Dto.Dtos;

public record HebWordDto(
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
    string[] Tags);
