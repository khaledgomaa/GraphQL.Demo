using GraphQL.Demo.DTOs;

namespace GraphQL.Demo.Services
{
    public interface IGenericRepository<TEntity> where TEntity : IBaseEntity<Guid>
    {
        Task<TEntity> Create(TEntity entity);

        Task<TEntity> Update(TEntity entity);

        Task<bool> Delete(Guid id);

        Task<IEnumerable<TEntity>> GetAll();

        Task<TEntity> GetById(Guid id);
    }
}
