using Messaging.Application.Features;
using Messaging.Domain.Entities;
using Messaging.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Api.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IRepository<Message> _messageRepository;

        public MessageController(
            ILogger<MessageController> logger,
            IRepository<Message> userRepository
            )
        {
            _logger = logger;
            _messageRepository = userRepository;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(string chatId, string content)
        {
            var message = new Message
            {
                UserId = 1,
                ChatId = chatId,
                Content = content
            };

            await _messageRepository.CreateAsync(message);

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
        public async Task<IActionResult> Get(string id)
        {
            var user = await _messageRepository.GetAsync(id);
            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, user));
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
