using MoreMath.Dto.Responses;

namespace MoreMath.App.ViewModels;

public class HebWordCardViewModel(HebWordPageItem word, bool isHidden)
{
    public HebWordPageItem Word { get; set; } = word;
    public bool IsHidden { get; set; } = isHidden;

    public void Reveal()
    {
        IsHidden = false;
    }
}