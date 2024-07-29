
namespace Auth.Application.DTOs
{
    public class UserGetAllRequestDto
    {
        public List<int>? UserIds { get; set; }
        public string? Filter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
