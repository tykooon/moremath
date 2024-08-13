using MoreMath.Core.Abstracts;
using System.Text.Json.Serialization;

namespace MoreMath.Core.Entities;

public class Author : EntityWithDates<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Uri? Avatar { get; set; }
    public string Info { get; set; } = string.Empty;
    public string ShortBio { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Article> Articles { get; set; } = [];

    public override string ToString() => $"Author: {FirstName} {LastName}";
}
