
using Messaging.Domain.Attributes;
using Messaging.Domain.Entities.Base;
using Messaging.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messaging.Domain.Entities
{
    [Table("chats")]
    public class Chat : Entity
    {
        [BsonElementName("name")]
        public string? Name { get; set; }

        [BsonElementName("type")]
        public ChatType Type { get; set; }

        [BsonElementName("users")]
        public List<ChatUser> Users { get; set; }

        [BsonElementName("messages")]
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
