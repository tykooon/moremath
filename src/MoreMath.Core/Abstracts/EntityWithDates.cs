namespace MoreMath.Core.Abstracts;

public abstract class EntityWithDates<TKey> : BaseEntity<TKey> where TKey : struct, IEquatable<TKey>
{
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Modified { get; set; } = DateTime.UtcNow;

    public void UpdateTimeMark() => Modified = DateTime.UtcNow;
}
