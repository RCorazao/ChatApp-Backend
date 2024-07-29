using Auth.Application.DTOs;
using Auth.Application.Identity;
using Auth.Persistence.DataBase;
using Auth.Persistence.Helpers;
using Auth.Persistence.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Auth.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApplicationUserDto> FindByIdAsync(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<ApplicationUserDto>(user);
        }

        public async Task<ApplicationUserDto> FindByEmailAsync(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<ApplicationUserDto>(user);
        }

        public async Task<List<ApplicationUserDto>> GetUsersAsync(UserGetAllRequestDto request)
        {
            var filter = request.Filter?.ToLower();

            var query = _context.Users.AsQueryable();

            if (!request.UserIds.IsNullOrEmpty())
            {
                query = query.Where(u => request.UserIds.Contains(u.Id));
            }

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(u => u.FullName.ToLower().Contains(filter));
            }

            request.PageNumber = (request.PageNumber < 1) ? 1 : request.PageNumber;
            int skip = (request.PageNumber - 1) * request.PageSize;
            query = query.Skip(skip).Take(request.PageSize);

            var users = await query.ToListAsync();

            return _mapper.Map<List<ApplicationUserDto>>(users);
        }
    }
}
