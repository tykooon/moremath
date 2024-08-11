using MoreMath.Core.Abstracts;

namespace MoreMath.Core.Entities;

public class Comment : EntityWithDates<int>
{
    public User? User { get; set; }
    public Article? Article { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}
