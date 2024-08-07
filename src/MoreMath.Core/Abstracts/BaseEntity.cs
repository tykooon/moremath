namespace MoreMath.Core.Abstracts;

public abstract class BaseEntity<TKey> where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; set; }
}
