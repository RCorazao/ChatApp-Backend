
using System.ComponentModel.DataAnnotations;

namespace Messaging.Application.DTOs
{
    public class SearchChatsRequestDto
    {
        [Required]
        public string Filter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
