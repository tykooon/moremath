using MoreMath.Core.Abstracts;

namespace MoreMath.Core.Entities;

public class Category : BaseEntity<int>
{
    public string CategoryName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
