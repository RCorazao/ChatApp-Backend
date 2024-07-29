
using Messaging.Application.DTOs;

namespace Messaging.Application.Interfaces
{
    public interface IUserServiceProxy
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<List<UserDto>?> GetUsersAsync(GetChatsProxyRequestDto request);
    }
}
