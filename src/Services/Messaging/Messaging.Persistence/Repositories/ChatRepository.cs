using Messaging.Domain.Entities;
using Messaging.Domain.Enums;
using Messaging.Domain.Repositories;
using Messaging.Persistence.Configurations;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Messaging.Persistence.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        public ChatRepository(MongoDBConfiguration context) : base(context)
        {
        }

        public async Task<bool> AddMessageAsync(string chatId, Message message)
        {
            var filter = Builders<Chat>.Filter.Eq(c => c.Id, chatId);
            var update = Builders<Chat>.Update.PushEach(c => c.Messages, new[] { message }, position: 0);
            var result = await _collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }

        public async Task<Chat?> GetContactChatAsync(int currentUserId, int contactId)
        {
            var filter = Builders<Chat>.Filter.And(
                Builders<Chat>.Filter.Eq(c => c.Type, ChatType.Private),
                Builders<Chat>.Filter.ElemMatch(c => c.Users, user => user.Id == currentUserId),
                Builders<Chat>.Filter.ElemMatch(c => c.Users, user => user.Id == contactId)
            );

            var projection = Builders<Chat>.Projection
                .Include(c => c.Id)
                .Include(c => c.Name)
                .Include(c => c.Type)
                .Include(c => c.Users)
                .Slice(c => c.Messages, 0, 1); // Get recent message

            var chatBson = await _collection.Find(filter)
                                        .Project(projection)
                                        .FirstOrDefaultAsync();

            if (chatBson == null)
            {
                return null;
            }

            var chat = BsonSerializer.Deserialize<Chat>(chatBson);
            return chat;
        }

        public async Task<Chat?> GetContactChatPaginatedAsync(int currentUserId, string chatId, int pageNumber, int pageSize)
        {
            var filter = Builders<Chat>.Filter.And(
                Builders<Chat>.Filter.Eq(c => c.Id, chatId),
                Builders<Chat>.Filter.ElemMatch(c => c.Users, user => user.Id == currentUserId)
            );

            pageNumber = (pageNumber < 1) ? 1 : pageNumber;
            var projection = Builders<Chat>.Projection
                .Include(c => c.Id)
                .Include(c => c.Name)
                .Include(c => c.Type)
                .Include(c => c.Users)
                .Slice(c => c.Messages, (pageNumber - 1) * pageSize, pageSize); // Paginate messages

            var chatBson = await _collection.Find(filter)
                                            .Project(projection)
                                            .FirstOrDefaultAsync();

            if (chatBson == null)
            {
                return null;
            }

            var chat = BsonSerializer.Deserialize<Chat>(chatBson);
            return chat;
        }

        public async Task<Chat?> GetMessagesChatAsync(int currentUserId, string chatId, int skip, int pageSize)
        {
            var filter = Builders<Chat>.Filter.And(
                Builders<Chat>.Filter.Eq(c => c.Id, chatId),
                Builders<Chat>.Filter.ElemMatch(c => c.Users, user => user.Id == currentUserId)
            );

            var chatBson = await _collection.Find(filter)
                                            .Project(Builders<Chat>.Projection
                                                .Include(c => c.Id)
                                                .Include(c => c.Name)
                                                .Include(c => c.Type)
                                                .Include(c => c.Users)
                                                .Slice(c => c.Messages, skip, pageSize)) // Slice from start after messageId
                                            .FirstOrDefaultAsync();

            if (chatBson == null)
            {
                return null;
            }

            var chat = BsonSerializer.Deserialize<Chat>(chatBson);
            return chat;
        }


        public async Task<List<Chat>> GetUserChatsAsync(int userId)
        {
            var filter = Builders<Chat>.Filter.ElemMatch(c => c.Users, user => user.Id == userId);

            var projection = Builders<Chat>.Projection
                .Include(c => c.Id)
                .Include(c => c.Name)
                .Include(c => c.Type)
                .Include(c => c.Users)
                .Slice(c => c.Messages, 0, 1);

            var chatBsonDocuments = await _collection.Find(filter)
                                                      .Project(projection)
                                                      .ToListAsync();

            return chatBsonDocuments.Select(doc => BsonSerializer.Deserialize<Chat>(doc)).ToList();
        }
    }
}
