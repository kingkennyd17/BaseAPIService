using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Base;
using Fintrak.Shared.Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Data.SystemCore
{
    //public class DataRepositoryBase<TEntity> : DataRepositoryBase<TEntity>
    //    where TEntity : class, IIdentifiableEntity, new()
    //{
    //    public DataRepositoryBase(SystemCoreDbContext context) : base(context)
    //    {
    //    }
    //}

    public class DataRepositoryBase<TEntity> : IDataRepository<TEntity> where TEntity : class
    {
        protected readonly SystemCoreDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public DataRepositoryBase(SystemCoreDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
