namespace MoreMath.Api.Requests.HebWords;

public record CreateHebWordRequest(
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
