using System.Linq.Expressions;

namespace SecretBot.DataAccess.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?> FindFirstAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        bool tracking = false,
        IEnumerable<Expression<Func<TEntity, object>>>? includes = null);

    IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        IEnumerable<Expression<Func<TEntity, object>>>? includes = null,
        int? page = null,
        int? pageSize = null,
        bool tracking = false);

    Task AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task UpdateAsync(TEntity entity);

    Task RemoveAsync(TEntity entity);

    Task RemoveRangeAsync(IEnumerable<TEntity> entities);

    Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null);
}