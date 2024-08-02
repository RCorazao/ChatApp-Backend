using Messaging.Application.DTOs;
using Messaging.Application.Features;
using Messaging.Application.Interfaces;
using Messaging.Domain.Entities;
using Messaging.Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chats")]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private readonly IRepository<Chat> _chatRepository;
        private readonly IUserService _userService;
        private readonly IChatService _chatService;

        public ChatController(
            ILogger<ChatController> logger,
            IRepository<Chat> userRepository,
            IUserService userService,
            IChatService chatService
            )
        {
            _logger = logger;
            _chatRepository = userRepository;
            _userService = userService;
            _chatService = chatService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(int userId)
        {
            UserDto user = _userService.GetUserFromClaims();

            if (user.Id == userId)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, message: "Invalid user"));
            }

            var chat = await _chatService.CreatePrivate(user, userId);

            if (chat == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, message: "Invalid user"));
            }

            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, chat));
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            UserDto user = _userService.GetUserFromClaims();

            var chats = await _chatService.GetChats(user);

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, chats));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Get(string id, [FromBody] ChatRequestDto request)
        {
            var user = _userService.GetUserFromClaims();

            var chat = await _chatService.GetPaginated(user, id, request.PageNumber, request.PageSize);

            if (chat == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, message: "Invalid chat"));
            }

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, chat));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _chatRepository.RemoveAsync(id);
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, message: "Record deleted"));
        }
    }
}
