
using Messaging.Domain.Entities;

namespace Messaging.Domain.Repositories
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<bool> AddMessageAsync(string chatId, Message message);
        Task<Chat?> GetContactChatAsync(int currentUserId, int contactId);
        Task<Chat?> GetContactChatPaginatedAsync(int currentUserId, string chatId, int pageNumber, int pageSize);
        Task<List<Chat>> GetUserChatsAsync(int userId);
    }
}
