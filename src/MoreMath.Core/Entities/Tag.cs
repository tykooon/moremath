using MoreMath.Core.Abstracts;

namespace MoreMath.Core.Entities;

public class Tag : BaseEntity<int>
{
    public string TagName { get; set; } = string.Empty;
}
