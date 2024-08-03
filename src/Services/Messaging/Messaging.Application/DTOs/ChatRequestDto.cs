
namespace Messaging.Application.DTOs
{
    public class ChatRequestDto
    {
        public int Skip { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
