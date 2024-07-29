
using Auth.Application.DTOs;

namespace Auth.Application.Identity
{
    public interface IUserService
    {
        Task<ApplicationUserDto> FindByIdAsync(string userId);
        Task<ApplicationUserDto> FindByEmailAsync(string email);
        Task<List<ApplicationUserDto>> GetUsersAsync(UserGetAllRequestDto request);
    }
}
