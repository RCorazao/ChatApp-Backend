
namespace Messaging.Application.DTOs
{
    public class NotificationDto
    {
        public bool IsCurrentUser { get; set; } = false;
        public ChatDto Chat { get; set; }
        public MessageDto Message { get; set; }
    }
}
