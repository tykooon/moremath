using MoreMath.Core.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoreMath.Core.Entities;

public class Author : EntityWithDates<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string Info { get; set; } = string.Empty;
    public string ShortBio { get; set; } = string.Empty;

    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? WhatsApp { get; set; }
    public string? Telegram { get; set; }
    public string? Website { get; set; }
    public string? Options { get; set; }

    [JsonIgnore]
    public ICollection<Article> Articles { get; set; } = [];

    public override string ToString() => $"{FirstName} {LastName}";
}
