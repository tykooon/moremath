using MoreMath.Core.Abstracts;

namespace MoreMath.Core.Entities;

public class User : EntityWithDates<int>
{
    public string Username { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public Author? Author { get; set; }

    public ICollection<Comment> Comments { get; set; } = [];
}
