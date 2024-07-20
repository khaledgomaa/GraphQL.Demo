namespace GraphQL.Demo.DTOs
{
    public interface IBaseEntity<TType>
    {
        TType Id { get; set; }
    }
}
