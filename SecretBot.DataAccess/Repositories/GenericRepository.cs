using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SecretBot.DataAccess.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DatabaseContext _context;

    public GenericRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> FindFirstAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        bool tracking = false,
        IEnumerable<Expression<Func<TEntity, object>>>? includes = null)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        if (includes != null)
        {
            foreach (var param in includes)
            {
                query = query.Include(param);
            }
        }

        return filter == null
            ? await query.FirstOrDefaultAsync()
            : await query.FirstOrDefaultAsync(filter);
    }

    public IQueryable<TEntity> FindAll(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        IEnumerable<Expression<Func<TEntity, object>>>? includes = null,
        int? page = null,
        int? pageSize = null,
        bool tracking = false)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        if (includes != null)
        {
            foreach (var param in includes)
            {
                query = query.Include(param);
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (pageSize != null && page != null)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return query;
    }

    public async Task AddAsync(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        await SaveAsync();
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
        await SaveAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await SaveAsync();
    }

    public async Task RemoveAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        await SaveAsync();
    }

    public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
        await SaveAsync();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        return filter != null
            ? await _context.Set<TEntity>().CountAsync(filter)
            : await _context.Set<TEntity>().CountAsync();
    }

    private async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
