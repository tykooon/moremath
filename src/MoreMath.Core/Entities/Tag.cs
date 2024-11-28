using Microsoft.EntityFrameworkCore;
using MoreMath.Core.Abstracts;
using System.Text.Json.Serialization;


namespace MoreMath.Core.Entities;

[Index(nameof(TagName))]
public class Tag : BaseEntity<int>
{
    public string TagName { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Article> Articles { get; set; } = [];

    [JsonIgnore]
    public ICollection<HebWord> HebWords { get; set; } = [];
}
