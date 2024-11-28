namespace MoreMath.Api.Requests.HebWords;

public record UpdateHebWordRequest(
    int Id,
    string? Shoresh,
    string? Niqqud,
    string? NoNiqqud,
    string? ExtraForm,
    string? Spelling,
    int? StressLetter,
    string? Translation,
    string? Notes,
    string? ExtraInfo);
