
using Messaging.Application.DTOs;
using Messaging.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Messaging.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserServiceProxy _userServiceProxy;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            IUserServiceProxy userServiceProxy
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _userServiceProxy = userServiceProxy;
        }

        public UserDto GetUserFromClaims()
        {
            var userClaims = _httpContextAccessor.HttpContext.User.Claims;

            var userDto = new UserDto
            {
                Id = int.Parse(userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0"),
                FullName = userClaims.FirstOrDefault(c => c.Type == "FullName")?.Value,
                Email = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                Avatar = userClaims.FirstOrDefault(c => c.Type == "Avatar")?.Value
            };

            return userDto;
        }

        public async Task<dynamic> GetUserById(int id)
        {
            var result = await _userServiceProxy.GetUserByIdAsync(id);
            return result;
        }
    }
}
