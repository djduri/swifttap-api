namespace SWIFTTAP.Domain.Base;

public abstract class Entity : IEquatable<Entity>, IEntity
{
    public long Id { get; private init; }

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    protected Entity()
    {
    }

    public static bool operator ==(Entity? left, Entity? right) =>
            ReferenceEquals(left, right) || (left?.Id == right?.Id);

    // Operator porównania !=
    public static bool operator !=(Entity? left, Entity? right) =>
        !(left == right);

    // Porównanie obiektów po ID
    public override bool Equals(object? obj) =>
        obj is Entity entity && entity.Id == Id;

    // Implementacja IEquatable<Entity> porównująca po ID
    public bool Equals(Entity? other) =>
        other is not null && other.Id == Id;

    // Generowanie HashCode na podstawie Id
    public override int GetHashCode() => Id.GetHashCode();
}
