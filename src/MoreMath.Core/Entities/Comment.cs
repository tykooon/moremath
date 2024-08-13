using MoreMath.Core.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreMath.Core.Entities;

public class Comment : EntityWithDates<int>
{
    [ForeignKey("User")]
    public int UserId { get; set; }

    public User? User { get; set; }

    [ForeignKey("Article")]
    public int ArticleId { get; set; }

    public Article? Article { get; set; }

    public string Text { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}
