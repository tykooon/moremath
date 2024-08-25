using MoreMath.Core.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoreMath.Core.Entities;

public class User : EntityWithDates<int>
{
    public string Username { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    [ForeignKey("Author")]
    public int? AuthorId { get; set; }

    public Author? Author { get; set; }

    [JsonIgnore]
    [InverseProperty("User")]
    public ICollection<Comment> Comments { get; set; } = [];
}
