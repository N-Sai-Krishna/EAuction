using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;


namespace EAuction.Persistence
{
    public interface IRepository<TEntity, TKey>
    {
        IQueryable<TEntity> Query();

        IAggregateFluent<TEntity> Aggregate();

        Task<TEntity> FindByAsync(TKey key);

        Task<TEntity> AddAsync (TEntity entity);
        Task<bool> UpdateAsync (TEntity entity);
        Task<bool> DeleteAsync (TKey key);
    }
}
