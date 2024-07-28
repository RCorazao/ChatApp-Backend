
using Messaging.Domain.Entities;
using Messaging.Domain.Entities.Base;
using Messaging.Domain.Repositories;
using Messaging.Persistence.Configurations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace Messaging.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly IMongoCollection<T> _collection;

        public Repository(MongoDBConfiguration context)
        {
            var tableAttribute = (TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute));
            _collection = context.GetCollection<T>(tableAttribute.Name.ToLower() ?? nameof(T).ToLower());
        }

        public async Task<T> CreateAsync(T entity)
        {
            entity.Id = ObjectId.GenerateNewId().ToString();
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, List<string> excludeFields)
        {
            var projectionBuilder = Builders<T>.Projection;
            ProjectionDefinition<T> projection = Builders<T>.Projection.Include("_id");

            foreach (var field in excludeFields)
            {
                projection = projectionBuilder.Exclude(field);
            }

            var bsonDocuments = await _collection.Find(filter).Project(projection).ToListAsync();

            return bsonDocuments.Select(doc => BsonSerializer.Deserialize<T>(doc)).ToList();
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
