using Microsoft.EntityFrameworkCore;
using MoreMath.Core.Abstracts;

namespace MoreMath.Core.Entities;

[Index(nameof(Shoresh), nameof(NoNiqqud), nameof(Spelling))]
public class HebWord: BaseEntity<int>
{
    public string Shoresh { get; set; } = "";
    public string Niqqud { get; set; } = "";
    public string NoNiqqud { get; set; } = "";
    public string ExtraForm { get; set; } = "";
    public string Spelling { get; set; } = "";
    public int StressLetter { get; set; } = -1;
    public string Translation { get; set; } = "";
    public string Notes { get; set; } = "";
    public string ExtraInfo { get; set; } = "";
    public ICollection<Tag> Tags { get; set; } = [];

}
