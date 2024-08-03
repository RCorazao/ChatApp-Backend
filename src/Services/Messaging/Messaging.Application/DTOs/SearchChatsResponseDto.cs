
using Messaging.Domain.Enums;

namespace Messaging.Application.DTOs
{
    public class SearchChatsResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public bool IsContact { get; set; }
    }
}
