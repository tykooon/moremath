using MoreMath.Core.Abstracts;

namespace MoreMath.Core.Entities;

public class Article : EntityWithDates<int>
{
    public string Title { get; set; } = string.Empty;
    public string Abstract {  get; set; } = string.Empty;
    public Uri? Body { get; set; }
    public List<Author> Authors { get; set; } = [];
    public Category? Category { get; set; }

    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];
}
