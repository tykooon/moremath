namespace MoreMath.Shared.Models;

public class HebWordFilter
{
    public string[] Tags { get; set; } = [];
    public string? Word { get; set; }
    public string? Translation { get; set; }
    public string? Shoresh { get; set; }
    public bool HasAllTags = false;
    public string SortBy { get; set; } = "";
    public bool SortAscening { get; set; } = true;
}
