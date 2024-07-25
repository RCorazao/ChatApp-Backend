
using Messaging.Domain.Attributes;
using Messaging.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messaging.Domain.Entities
{
    [Table("messages")]
    public class Message : Entity
    {
        [BsonElementName("user_id")]
        public int UserId { get; set; }

        [BsonElementName("chat_id")]
        public string ChatId { get; set; }

        [BsonElementName("content")]
        public string Content { get; set; } = string.Empty;

        [BsonElementName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
