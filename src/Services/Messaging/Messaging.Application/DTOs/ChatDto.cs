
using Messaging.Domain.Enums;

namespace Messaging.Application.DTOs
{
    public class ChatDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ChatType Type { get; set; }
        public List<UserDto> Users { get; set; }
        public List<MessageDto> Messages { get; set; } = new List<MessageDto>();
    }
}
