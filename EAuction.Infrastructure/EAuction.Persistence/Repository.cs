using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAuction.Persistence
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        protected readonly IMongoCollection<TEntity> collection;

        public Repository(IMongoDatabase mongoDatabase)
        {
            collection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);
        }
        
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await this.collection.InsertOneAsync(entity);
            return entity;
;        }

        public virtual IAggregateFluent<TEntity> Aggregate()
        {
            return this.collection.Aggregate();
        }

        public virtual async Task<bool> DeleteAsync(TKey key)
        {
            await this.collection.DeleteOneAsync(s => s.Id.Equals(key));
            return true;
        }

        public virtual async Task<TEntity> FindByAsync(TKey key)
        {
            var result = await this.collection.FindAsync<TEntity>( i => i.Id.Equals(key));
            return result.FirstOrDefault();
        }

        public virtual IQueryable<TEntity> Query()
        {
            return this.collection.AsQueryable();
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(s => s.Id, entity.Id);
            await this.collection.FindOneAndReplaceAsync(filter, entity);
            return true;
        }
    }
}
