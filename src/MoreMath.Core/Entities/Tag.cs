using MoreMath.Core.Abstracts;
using System.Text.Json.Serialization;

namespace MoreMath.Core.Entities;

public class Tag : BaseEntity<int>
{
    public string TagName { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Article> Articles { get; set; } = [];
}
