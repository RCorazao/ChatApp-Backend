using Messaging.Domain.Attributes;
using Messaging.Domain.Entities.Base;
using Messaging.Persistence.Helpers;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Reflection;

namespace Messaging.Persistence.Configurations
{
    public class MongoDBConfiguration
    {
        private readonly IMongoDatabase _database;

        public MongoDBConfiguration(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDB:ConnectionString").Value;
            var databaseName = configuration.GetSection("MongoDB:Database").Value;

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);

            MapAllEntities();
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        private void MapAllEntities()
        {
            var entityTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IEntity).IsAssignableFrom(t));

            foreach (var type in entityTypes)
            {
                MapClass(type);
            }
        }

        private void MapClass(Type type)
        {
            if (!BsonClassMap.IsClassMapRegistered(type))
            {
                var classMapType = typeof(BsonClassMap<>).MakeGenericType(type);
                var classMap = Activator.CreateInstance(classMapType) as BsonClassMap;

                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);

                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    if (property.Name == "Id") continue;

                    var elementName = GetElementName(property);

                    classMap.MapMember(property).SetElementName(elementName);
                }

                BsonClassMap.RegisterClassMap(classMap);
            }
        }

        private string GetElementName(System.Reflection.PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<BsonElementNameAttribute>();
            if (attribute != null)
            {
                return attribute.ElementName;
            }

            return property.Name.ToSnakeCase();
        }
    }
}
