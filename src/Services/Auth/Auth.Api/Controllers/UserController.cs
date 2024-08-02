using Auth.Application.DTOs;
using Auth.Application.Features;
using Auth.Application.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(
            ILogger<UserController> logger,
            IUserService userService
            )
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] UserGetAllRequestDto request)
        {
            var users = await _userService.GetUsersAsync(request);

            if (users == null || users.Count == 0)
                return StatusCode(StatusCodes.Status404NotFound,
                   ResponseApiService.Response(StatusCodes.Status404NotFound, message: "Users not found"));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, users));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var users = await _userService.FindByIdAsync(id.ToString());

            if (users == null)
                return StatusCode(StatusCodes.Status404NotFound,
                   ResponseApiService.Response(StatusCodes.Status404NotFound));

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, users));
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser(int id)
        {
            var userClaims = User.Claims;

            var user = new ApplicationUserDto
            {
                Id = int.Parse(userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value),
                FullName = userClaims.FirstOrDefault(c => c.Type == "FullName")?.Value,
                Email = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                Avatar = userClaims.FirstOrDefault(c => c.Type == "Avatar")?.Value
            };

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, user));
        }
    }
}
