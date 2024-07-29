using Auth.Application.DTOs;
using Auth.Application.Features;
using Auth.Application.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
                   ResponseApiService.Response(StatusCodes.Status404NotFound));

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
    }
}
