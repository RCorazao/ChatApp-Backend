
using Messaging.Domain.Attributes;
using Messaging.Domain.Entities.Base;
using Messaging.Domain.Enums;

namespace Messaging.Domain.Entities
{
    public class ChatUser : IEntity
    {
        [BsonElementName("id")]
        public int Id { get; set; }

        [BsonElementName("rol")]
        public UserRol Rol { get; set; }


        public ChatUser(int userId) 
        {
            Id = userId;
            Rol = UserRol.None;
        }
    }
}
