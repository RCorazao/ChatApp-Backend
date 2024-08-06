
using Messaging.Domain.Attributes;

namespace Messaging.Application.DTOs
{
    public class MessageDto
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public string ChatId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
