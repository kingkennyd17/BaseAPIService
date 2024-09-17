using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fintrak.Shared.Common.Interface
{
    //public interface IDataRepository
    //{

    //}

    //public interface IDataRepositoryBase<TEntity>
    //    where TEntity : class
    //public interface IDataRepository<TEntity> 
    //    where TEntity : class, IIdentifiableEntity, new()
    //{
    //    Task<IEnumerable<TEntity>> GetAllAsync();
    //    Task<TEntity> GetByIdAsync(object id);
    //    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    //    Task AddAsync(TEntity entity);
    //    Task AddRangeAsync(IEnumerable<TEntity> entities);
    //    void Update(TEntity entity);
    //    void Remove(TEntity entity);
    //    void RemoveRange(IEnumerable<TEntity> entities);
    //}
    public interface IDataRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
    }
}
