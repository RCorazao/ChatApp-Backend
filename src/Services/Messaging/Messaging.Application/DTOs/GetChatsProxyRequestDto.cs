
namespace Messaging.Application.DTOs
{
    public class GetChatsProxyRequestDto
    {
        public List<int>? UserIds { get; set; }
        public string? Filter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
