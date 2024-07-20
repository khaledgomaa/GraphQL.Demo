using GraphQL.Demo.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Demo.Services
{
    public class GenericRepository<TEntity>(IServiceScopeFactory serviceScopeFactory) : IGenericRepository<TEntity> where TEntity : class, IBaseEntity<Guid>
    {
        public async Task<TEntity> Create(TEntity entity)
        {
            using var appDbContext = serviceScopeFactory.CreateAsyncScope()
                .ServiceProvider.GetRequiredService<AppDbContext>();

            await appDbContext.AddAsync(entity);

            await appDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            using var appDbContext = serviceScopeFactory.CreateAsyncScope()
                .ServiceProvider.GetRequiredService<AppDbContext>();

            appDbContext.Update(entity);

            await appDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            using var appDbContext = serviceScopeFactory.CreateAsyncScope()
                .ServiceProvider.GetRequiredService<AppDbContext>();

            var dbSet = appDbContext.Set<TEntity>();

            TEntity entity = await dbSet.FindAsync(id);

            dbSet.Remove(entity);

            return await appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            using var appDbContext = serviceScopeFactory.CreateAsyncScope()
                .ServiceProvider.GetRequiredService<AppDbContext>();

            var dbSet = appDbContext.Set<TEntity>();

            return await dbSet
                        .ToListAsync();
        }

        public async Task<TEntity> GetById(Guid id)
        {
            using var appDbContext = serviceScopeFactory.CreateAsyncScope()
                .ServiceProvider.GetRequiredService<AppDbContext>();

            var dbSet = appDbContext.Set<TEntity>();

            return await dbSet
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<TEntity>> GetByIds(IEnumerable<Guid> ids)
        {
            using var appDbContext = serviceScopeFactory.CreateAsyncScope()
                .ServiceProvider.GetRequiredService<AppDbContext>();

            var dbSet = appDbContext.Set<TEntity>();

            return await dbSet
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();
        }
    }
}