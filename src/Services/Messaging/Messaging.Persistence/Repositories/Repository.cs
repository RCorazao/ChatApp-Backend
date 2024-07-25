
using Messaging.Domain.Entities.Base;
using Messaging.Domain.Repositories;
using Messaging.Persistence.Configurations;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace Messaging.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;

        public Repository(MongoDBConfiguration context)
        {
            var tableAttribute = (TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute));
            _collection = context.GetCollection<T>(tableAttribute.Name.ToLower() ?? nameof(T).ToLower());
        }

        public async Task CreateAsync(T entity)
        {
            entity.Id = ObjectId.GenerateNewId().ToString();
            await _collection.InsertOneAsync(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }

        public async Task<bool> RemoveAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(e => e.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
