using Fintrak.Shared.Common.Interface;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fintrak.Shared.Common.Base
{
    //public class DataRepositoryBase<TContext, TEntity> : IDataRepository<TEntity>
    //    where TEntity : class, IIdentifiableEntity, new()
    //    where TContext : IdentityDbContext, new()
    //{
    //    protected readonly TContext _context;
    //    private readonly DbSet<TEntity> _entities;

    //    public DataRepositoryBase(TContext context)
    //    {
    //        _context = context;
    //        _entities = context.Set<TEntity>();
    //    }

    //    public async Task<IEnumerable<TEntity>> GetAllAsync()
    //    {
    //        return await _entities.ToListAsync();
    //    }

    //    public async Task<TEntity> GetByIdAsync(object id)
    //    {
    //        return await _entities.FindAsync(id);
    //    }

    //    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    //    {
    //        return await _entities.Where(predicate).ToListAsync();
    //    }

    //    public async Task AddAsync(TEntity entity)
    //    {
    //        await _entities.AddAsync(entity);
    //    }

    //    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    //    {
    //        await _entities.AddRangeAsync(entities);
    //    }

    //    public void Update(TEntity entity)
    //    {
    //        _entities.Update(entity);
    //    }

    //    public void Remove(TEntity entity)
    //    {
    //        _entities.Remove(entity);
    //    }

    //    public void RemoveRange(IEnumerable<TEntity> entities)
    //    {
    //        _entities.RemoveRange(entities);
    //    }
    //}

  
}
