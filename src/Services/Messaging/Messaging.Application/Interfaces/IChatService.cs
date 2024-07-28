
using Messaging.Application.DTOs;

namespace Messaging.Application.Interfaces
{
    public interface IChatService
    {
        Task<ChatDto> CreatePrivate(UserDto user, int contactId);
        Task<List<ChatDto>> GetChats(UserDto user);
        Task<ChatDto> GetPaginated(UserDto user, string chatId, int pageNumber, int pageSize);
    }
}
