namespace Domain.Primitives.Interfaces
{
    public class IEntity
    {
        Guid Id { get; set; }
        DateTime InsertDate { get; set; }
        DateTime UpdateDate { get; set; }
    }
}
