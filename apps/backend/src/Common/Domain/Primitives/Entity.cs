using Domain.Primitives.Interfaces;

namespace Domain.Primitives;

public abstract class Entity : IEntity
{
    public Guid Id { get; private init; }
    public DateTime InsertDate { get; private init; }
    public DateTime UpdateDate { get; private init; }

    protected Entity(Guid id)
    {
        Id = id;
        InsertDate = DateTime.Now;
        UpdateDate = DateTime.Now;
    }

    protected Entity() { }
}
