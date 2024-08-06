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
        private readonly IMessageService _messageService;
        private readonly INotificationService _notificationService;

        public MessageController(
            ILogger<MessageController> logger,
            IRepository<Message> userRepository,
            IChatRepository chatRepository,
            IUserService userService,
            IMessageService messageService,
            INotificationService notificationService
            )
        {
            _logger = logger;
            _messageRepository = userRepository;
            _chatRepository = chatRepository;
            _userService = userService;
            _messageService = messageService;
            _notificationService = notificationService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] MessageRequestDto request)
        {
            UserDto user = _userService.GetUserFromClaims();

            var result = await _messageService.Create(user, request.ChatId, request.Content);

            if (result is null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                ResponseApiService.Response(StatusCodes.Status400BadRequest, message: "Invalid chat"));
            }

            return StatusCode(StatusCodes.Status201Created,
                ResponseApiService.Response(StatusCodes.Status201Created, result));
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
