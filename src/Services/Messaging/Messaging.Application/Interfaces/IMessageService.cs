
using Messaging.Application.DTOs;

namespace Messaging.Application.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDto?> Create(UserDto sender, string chatId, string content);
    }
}
