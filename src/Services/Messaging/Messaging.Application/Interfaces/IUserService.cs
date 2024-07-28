
using Messaging.Application.DTOs;

namespace Messaging.Application.Interfaces
{
    public interface IUserService
    {
        UserDto GetUserFromClaims();
    }
}
