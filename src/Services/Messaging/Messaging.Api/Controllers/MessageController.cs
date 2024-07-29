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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IRepository<Message> _messageRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IUserService _userService;

        public MessageController(
            ILogger<MessageController> logger,
            IRepository<Message> userRepository,
            IChatRepository chatRepository,
            IUserService userService
            )
        {
            _logger = logger;
            _messageRepository = userRepository;
            _chatRepository = chatRepository;
            _userService = userService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(string chatId, string content)
        {
            UserDto user = _userService.GetUserFromClaims();
            var chat = await _chatRepository.GetAsync( chatId );

            if ( chat == null )
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, message: "Invalid chat"));
            }

            var message = new Message
            {
                UserId = user.Id,
                ChatId = chatId,
                Content = content
            };

            var record = await _messageRepository.CreateAsync(message);

            await _chatRepository.AddMessageAsync(chatId, record);

            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, message));
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var users = await _messageRepository.GetAllAsync();
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, users));
        }

        [HttpGet("{id}")]
        public async Task<dynamic> Get(int id)
        {
            var result = await _userService.GetUserById(id);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _messageRepository.RemoveAsync(id);
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, message: "Record deleted"));
        }
    }
}
