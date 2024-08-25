using MoreMath.Core.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoreMath.Core.Entities;

public class Article : EntityWithDates<int>
{
    public string Title { get; set; } = string.Empty;
    public string Abstract {  get; set; } = string.Empty;
    public string BodyUri { get; set; } = string.Empty;

    public ICollection<Author> Authors { get; set; } = [];

    [ForeignKey("Category")]
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    [JsonIgnore]
    [InverseProperty("Article")]
    public ICollection<Comment> Comments { get; set; } = [];

    [InverseProperty("Articles")]
    public ICollection<Tag> Tags { get; set; } = [];
}
